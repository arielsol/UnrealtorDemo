using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

public class MatchSpawner : MonoBehaviour
{
    public float floatTime = 3f;
    public float geoSize = 1f;
    public float sizeScalar = 8f;
    public GameObject[] prefabs;
    public Material[] colors;

    public void SpawnMatch(QuadSOPair pair)
    {
        Vector3 positionLeft = new Vector3(pair.leftQuad.spawnPoint.x, pair.leftQuad.spawnPoint.y, pair.leftQuad.spawnPoint.z);
        Vector3 positionRight = new Vector3(pair.rightQuad.spawnPoint.x, pair.rightQuad.spawnPoint.y, pair.rightQuad.spawnPoint.z);

        GameObject cubeLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeLeft.transform.localScale = new Vector3(pair.leftQuad.size.x, pair.leftQuad.size.y, pair.leftQuad.size.z);
        cubeLeft.transform.position = positionLeft;
        StartCoroutine(LateAddRigidbody(cubeLeft));

        GameObject cubeRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeRight.transform.localScale = new Vector3(pair.rightQuad.size.x, pair.rightQuad.size.y, pair.rightQuad.size.z);
        cubeRight.transform.position = positionRight;
        StartCoroutine(LateAddRigidbody(cubeRight));
    }

    public void SpawnInfiniteMatch(QuadSOPair pair)
    {
        Vector3 positionLeft = new Vector3(pair.leftQuad.spawnPoint.x, pair.leftQuad.spawnPoint.y, pair.leftQuad.spawnPoint.z);
        Vector3 positionRight = new Vector3(pair.rightQuad.spawnPoint.x, pair.rightQuad.spawnPoint.y, pair.rightQuad.spawnPoint.z);
        float randomSize = UnityEngine.Random.Range(0.1f, geoSize) * sizeScalar;
        int randomIndex = Random.Range(0, prefabs.Length);
        GameObject selectedPrefab = prefabs[randomIndex];

        // Spawn on left side
        GameObject geoLeft = Instantiate(selectedPrefab, positionLeft, Quaternion.identity);
        geoLeft.transform.localScale = new Vector3 (randomSize, randomSize, randomSize);

        // Spawn on right side
        GameObject geoRight = Instantiate(selectedPrefab, positionRight, Quaternion.identity);
        geoRight.transform.localScale = new Vector3 (randomSize, randomSize, randomSize);

        /*
        GameObject cubeLeft = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeLeft.transform.localScale = new Vector3 (randomSize, randomSize, randomSize);
        cubeLeft.transform.position = positionLeft;
        RandomColor(cubeLeft);
        cubeLeft.AddComponent<Rigidbody>();

        GameObject cubeRight = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cubeRight.transform.localScale = new Vector3 (randomSize, randomSize, randomSize);
        cubeRight.transform.position = positionRight;
        RandomColor(cubeRight);
        cubeRight.AddComponent<Rigidbody>();
        */
    }

    private void RandomColor(GameObject obj)
    {
        Renderer rend = obj.GetComponent<Renderer>();
        
        int randomIndex = Random.Range(0, colors.Length);
        Material selectedMaterial = colors[randomIndex];
        
        if (rend != null)
        {
            rend.material = selectedMaterial;
        }
    }

    IEnumerator LateAddRigidbody(GameObject cube)
    {
        yield return new WaitForSeconds(floatTime);
        cube.AddComponent<Rigidbody>();
    }
}
