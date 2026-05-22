using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.Collections.AllocatorManager;


public class Mausclick : MonoBehaviour
{
    private GameObject previewBlock;
    private GameObject selectedPrefab;
    private bool isBuildingBeam = false;
    private Vector3 beamStartPos;
    private GameObject beamPreview;
    private bool beamMode = false;

    public GameObject beamPrefab;
    public GameObject squarePrefab;
    public GameObject circlePrefab;
    public GameObject trianglePrefab;
    public float gridSize = 0.25f;
    public float maxBeamLength = 5f;
    public List<GameObject> placedBlocks = new List<GameObject>();
    public GameObject previewPrefabS;
    public GameObject previewPrefabC;
    public GameObject previewPrefabT;
    public GameObject previewPrefab1B;
    public GameObject previewPrefab2B;

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
            beamMode = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedPrefab = circlePrefab;
            Destroy(previewBlock);
            previewBlock = Instantiate(previewPrefabC);
            beamMode = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedPrefab = trianglePrefab;
            Destroy(previewBlock);
            previewBlock = Instantiate(previewPrefabT);
            beamMode = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Destroy(previewBlock);
            previewBlock = Instantiate(previewPrefab1B);
            beamMode = true;
        }

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        worldPos.x = Mathf.Round(worldPos.x / gridSize) * gridSize;
        worldPos.y = Mathf.Round(worldPos.y / gridSize) * gridSize;
        worldPos.z = 0f;
        if (!isBuildingBeam)
        {
            previewBlock.transform.position = worldPos;
        }

        if (isBuildingBeam)
        {
            Vector3 direction = worldPos - beamStartPos;

            if (direction.magnitude > maxBeamLength)
            {
                direction = direction.normalized * maxBeamLength;
            }

            Vector3 limitedEndPos = beamStartPos + direction;

            UpdateBeamVisual(beamPreview, beamStartPos, limitedEndPos);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (beamMode)
            {
                if (!isBuildingBeam)
                {
                    beamStartPos = worldPos;
                    isBuildingBeam = true;
                    beamPreview = Instantiate(previewPrefab2B, beamStartPos, Quaternion.identity);
                }
                else
                {
                    Vector3 direction = worldPos - beamStartPos;

                    if (direction.magnitude > maxBeamLength)
                    {
                        direction = direction.normalized * maxBeamLength;
                    }

                    Vector3 limitedEndPos = beamStartPos + direction;

                    GameObject newBeam = Instantiate(beamPrefab);
                    UpdateBeamVisual(newBeam, beamStartPos, limitedEndPos);

                    Destroy(beamPreview);
                    beamPreview = null;
                    isBuildingBeam = false;
                }
            }
            else
            {
                Debug.Log(worldPos);

                //erstellte prefabs gehen direkt in die liste
                GameObject newBlock = Instantiate(selectedPrefab, worldPos, Quaternion.identity);
                placedBlocks.Add(newBlock);
            }
            

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
    void UpdateBeamVisual(GameObject beam, Vector3 start, Vector3 end)
    {
        Vector3 middle = (start + end) / 2f;
        Vector3 direction = end - start;

        beam.transform.position = middle;
        beam.transform.right = direction;
        beam.transform.localScale = new Vector3(direction.magnitude, 0.2f, 1f);
    }
}
