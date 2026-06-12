using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float velocidade = 10f;          // velocidade de corrida
    public float forcaPulo = 7f;            // força do pulo

    [Header("Configurações")]
    public string tagChao = "Chao";         // tag do objeto chão na cena

    private Rigidbody rb;
    private bool noChao = true;
    private bool jogoAtivo = true;

    void Start()
    {
        // Pega o componente Rigidbody anexado nesse mesmo objeto
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody não encontrado no personagem! Adicione o componente.");
        }
    }

    void Update()
    {
        if (!jogoAtivo) return; // para tudo se game over

        // Corre para frente continuamente
        transform.Translate(Vector3.forward * velocidade * Time.deltaTime);

        // Pulo — barra de espaço no PC ou toque na tela (mobile)
        bool pulouTeclado = Input.GetKeyDown(KeyCode.Space);
        bool pulouToque   = Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;

        if ((pulouTeclado || pulouToque) && noChao)
        {
            rb.AddForce(Vector3.up * forcaPulo, ForceMode.Impulse);
            noChao = false;
        }
    }

    // Detecta quando o personagem toca o chão
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagChao))
        {
            noChao = true;
        }
    }

    // Detecta colisão com obstáculo — chama game over
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstaculo"))
        {
            jogoAtivo = false;
            Debug.Log("Game Over!");

            // Aqui você pode chamar a tela de game over depois
            // GameManager.instance.GameOver();
        }
    }

    // Método público para aumentar velocidade (chamado pelo GameManager)
    public void AumentarVelocidade(float quantidade)
    {
        velocidade += quantidade;
    }
}
