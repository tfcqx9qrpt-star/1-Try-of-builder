using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Unity.Collections.AllocatorManager;


public class Mausclick : MonoBehaviour
{
    public GameObject squarePrefab;
    public GameObject circlePrefab;
    public GameObject trianglePrefab;

    private GameObject selectedPrefab;
    public float gridSize = 0.25f;
    public List<GameObject> placedBlocks = new List<GameObject>();
    public GameObject previewPrefabS;
    public GameObject previewPrefabC;
    public GameObject previewPrefabT;
    private GameObject previewBlock;
    Collider2D hit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        previewBlock = Instantiate(previewPrefabS);
        selectedPrefab = squarePrefab;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedPrefab = squarePrefab;
            Destroy(previewBlock);
            previewBlock = Instantiate(previewPrefabS);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedPrefab = circlePrefab;
            Destroy(previewBlock);
            previewBlock = Instantiate(previewPrefabC);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedPrefab = trianglePrefab;
            Destroy(previewBlock);
            previewBlock = Instantiate(previewPrefabT);
        }

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        worldPos.x = Mathf.Round(worldPos.x / gridSize) * gridSize;
        worldPos.y = Mathf.Round(worldPos.y / gridSize) * gridSize;
        worldPos.z = 0f;
        previewBlock.transform.position = worldPos;

        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log(worldPos);
          
            //erstellte prefabs gehen direkt in die liste
            GameObject newBlock = Instantiate(selectedPrefab, worldPos, Quaternion.identity);
            placedBlocks.Add(newBlock);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            foreach (GameObject block in placedBlocks)
            {
                Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;

            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            foreach (GameObject block in placedBlocks)
            {
                Rigidbody2D rb = block.GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            hit = Physics2D.OverlapPoint(worldPos);

            if (hit != null) 
            {
                if (placedBlocks.Contains(hit.gameObject))

                {
                    Destroy(hit.gameObject);
                    placedBlocks.Remove(hit.gameObject);
                }
               
            }
        }
    }
}
