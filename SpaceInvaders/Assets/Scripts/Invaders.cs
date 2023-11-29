using UnityEngine;

public class Invaders : MonoBehaviour
{
    [Header("Invaders")]
    public Invader[] prefabs = new Invader[5];
    public AnimationCurve speed = new AnimationCurve();
    public Vector3 direction { get; private set; } = Vector3.right;
    public Vector3 initialPosition { get; private set; }

    [Header("Grid")]
    public int rows = 5;
    public int columns = 11;

    [Header("Missiles")]
    public Projectile missilePrefab;
    public float missileSpawnRate = 1f;

    private void Awake()
    {
        initialPosition = transform.position;

        CreateInvaderGrid();
    }

    private void CreateInvaderGrid()
    {
        for (int i = 0; i < rows; i++)
        {
            float width = 2f * (columns - 1);
            float height = 2f * (rows - 1);

            Vector2 centerOffset = new Vector2(-width * 0.5f, -height * 0.5f);
            Vector3 rowPosition = new Vector3(centerOffset.x, (2f * i) + centerOffset.y, 0f);

            for (int j = 0; j < columns; j++)
            {

                // Crea un nuevo invader y lo hace hijo de esta transformacion
                Invader invader = Instantiate(prefabs[i], transform);

                // Calcula y establece la posición del invader
                Vector3 position = rowPosition;
                position.x += 2f * j;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(MissileAttack), missileSpawnRate, missileSpawnRate);
    }

    private void MissileAttack()
    {
        int amountAlive = GetAliveCount();

        // Los misiles no aparecerán cuando no haya invaders vivos
        if (amountAlive == 0) {
            return;
        }

        foreach (Transform invader in transform)
        {
            
            // Los invaders muertos no tirarán misiles
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            // Calcula la probabilidad de spawnear un misil dependiendo de cuando invaders haya
            // vivos (Cuantos más invaders menor probabilidad)
            if (Random.value < (1f / (float)amountAlive))
            {
                Instantiate(missilePrefab, invader.position, Quaternion.identity);
                break;
            }
        }
    }

    private void Update()
    {
        // Calcula el porcentaje de invaders vivos
        int totalCount = rows * columns;
        int amountAlive = GetAliveCount();
        int amountKilled = totalCount - amountAlive;
        float percentKilled = (float)amountKilled / (float)totalCount;

        // Establece la velocidad de los invaders dependiendo de cuantos hayan sido asesinados
        float speed = this.speed.Evaluate(percentKilled);
        transform.position += direction * speed * Time.deltaTime;


        // Transforma la zona visible de la pantalla a coordenadas de mundo para comprobar
        // cuando llegan al borde de la pantalla
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        // Los invaders avanzan a la siguiente fila una vez llegado al borde de la pantalla
        foreach (Transform invader in transform)
        {
            // Se ignoran los invaders que no sigan vivos
            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            // Se comprueban los bordes dependiendo de la dirección actual
            if (direction == Vector3.right && invader.position.x >= (rightEdge.x - 1f))
            {
                AdvanceRow();
                break;
            }
            else if (direction == Vector3.left && invader.position.x <= (leftEdge.x + 1f))
            {
                AdvanceRow();
                break;
            }
        }
    }

    private void AdvanceRow()
    {
        
        // Cambia la dirección de los invaders
        direction = new Vector3(-direction.x, 0f, 0f);

        // Mueve la fila de invaders 1 linea
        Vector3 position = transform.position;
        position.y -= 1f;
        transform.position = position;
    }

    public void ResetInvaders()
    {
        direction = Vector3.right;
        transform.position = initialPosition;

        foreach (Transform invader in transform) {
            invader.gameObject.SetActive(true);
        }
    }

    public int GetAliveCount()
    {
        int count = 0;

        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf) {
                count++;
            }
        }

        return count;
    }

}
