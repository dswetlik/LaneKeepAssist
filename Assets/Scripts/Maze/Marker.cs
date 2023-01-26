using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{

    public bool HasBeenVisited { get; private set; }


    private GameObject _offLight;
    private GameObject _onLight;

    // Start is called before the first frame update
    void Start()
    {
        _offLight = transform.GetChild(1).gameObject;
        _onLight = transform.GetChild(2).gameObject;

        _onLight.SetActive(false);

        StartCoroutine(Blink());
    }

    public void VisitedMarker() {
        HasBeenVisited = true;

        transform.GetChild(3).gameObject.SetActive(false);
        //transform.GetChild(4).gameObject.SetActive(false);

    }

    private IEnumerator Blink()
    {
        while(isActiveAndEnabled)
        {
            _offLight.SetActive(!_offLight.activeInHierarchy);
            _onLight.SetActive(!_onLight.activeInHierarchy);

            yield return new WaitForSecondsRealtime(1f);
        }
    }

}
