using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private LayerMask _buildMask;

    public Renderer MainRanderer;
    public Vector2 Size = Vector2.one;

    public void SetTranssparent(bool isAvailable)
    {
        if (isAvailable)
        {
        MainRanderer.material.color = Color.green;
        }
        else
        {
        MainRanderer.material.color = Color.red;
        }
    }

    public void SetNormal()
    {
        MainRanderer.material.color = Color.white;
       
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if ((x + y) % 2 == 0)
                    Gizmos.color = new Color(.88f, 0, 1, 0.3f);
                else
                    Gizmos.color = new Color(1, 0.68f, 0, 0.3f);

                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, .1f, 1));
            }
        }
    }
}