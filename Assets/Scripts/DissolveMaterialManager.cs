using UnityEngine;

public class DissolveMaterialManager : MonoBehaviour
{
    private Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        var value = Mathf.PingPong(Time.time * 0.5f, 1f);
        SetValue(value);
    }

    //IEnumerator enumerator()
    //{

    //    //float value =         while (true)
    //    //{
    //    //    Mathf.PingPong(value, 1f);
    //    //    value += Time.deltaTime;
    //    //    SetValue(value);
    //    //    yield return new WaitForEndOfFrame();
    //    //}
    //}

    public void SetValue(float value)
    {
        material.SetFloat("_Dissolve", value);
    }
}
