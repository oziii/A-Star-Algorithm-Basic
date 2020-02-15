using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int iGridX;//X - Y pozisyonlarını bulmak için
    public int iGridY;

    public bool bIsBlocked;//Düğüm yürünür mü yürünülmez mi kontrolü

    public Vector2 vPosition; //düğüm pozisyonu

    public Node ParentNode;//Yol çizilirken kendinden sonra gelecek düğüm

    public float iGvalue;//G değeri
    public float iHvalue;//H değeri

    public float fvalue { get { return iGvalue + iHvalue; } }//G ve H değerinin toplamı olan F değeri 

}
