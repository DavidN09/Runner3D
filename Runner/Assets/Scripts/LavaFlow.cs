using UnityEngine;

public class LavaOffset : MonoBehaviour
{
    public float scrollSpeedX = 0.2f;
    public float scrollSpeedY = 0.2f;
    public int materialIndex = 1; // Usa aquí el índice correcto de la lava

    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();
        offset = rend.materials[materialIndex].mainTextureOffset;
    }

    void Update()
    {
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;
        rend.materials[materialIndex].mainTextureOffset = offset;
    }
}
