using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDown : MonoBehaviour
{
    private void OnMouseDown()//Sol tık yapıldığında fonksiyon çalışır.
    {
        GameObject.Find("GameManager").GetComponent<PathFinding>().MousePos(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y),0);//Mouseun tıkladığı gameObjectin pozisyonu
    }
}
