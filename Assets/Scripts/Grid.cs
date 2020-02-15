using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("GameObject")]
    public GameObject gBox;//Kare düğümlerin gameObjecti
    public GameObject gHero;//Başlangıç noktası gameObjecti
    public GameObject gTarget;//Hedef noktanın gameObjecti

    public Node[,] Nodegrid;// A Star algoritmasının kullandığı düğüm dizisi.

    public int iGridWidth, iGridHeight;//oluşacak gridin değerleri
    public int iwallCount;//kaç tane duvar basacağımızı tutan değer

    void Start()
    {
        Nodegrid = new Node[iGridWidth, iGridHeight];//oluşacak grid büyüklüğünde 2 boyutlu dizi oluşturuyoruz

        createNodeGrid();
        createGrid();
        createWall();
    }

    void createNodeGrid()//Node Grid dizisini oluşturduğumuz düğümleri ekleyen fonksiyon
    {
        for (int x = 0; x < iGridWidth; x++)
        {
            for (int y = 0; y < iGridHeight; y++)
            {
                bool isWall = false;
                Nodegrid[x, y] = new Node()
                {
                    iGridX = x,
                    bIsBlocked = isWall,
                    iGridY = y
                };
            }
        }
    }

    void createGrid()//Ekran da kullanıcın gördüğü gBox şekli ile oluşturulan kare harita
    {
        for (int x = 0; x < iGridWidth; x++)
        {
            for (int y = 0; y < iGridHeight; y++)
            {
                GameObject nobj = (GameObject)GameObject.Instantiate(gBox);//Kareyi oluşturuyoruz.
                nobj.transform.position = new Vector2(x , y);//Karenin pozisyonunu veriyoruz.
                nobj.name = x + "," + y;//Kareler karışmasın diye pozisyonlarını adlarına da veriyoruz.

                nobj.gameObject.transform.parent = gBox.transform.parent;//Transform değerlerine de veriyoruz.
                nobj.SetActive(true);//Kare objeyi görünür hale getiriyoruz.
                Rigidbody2D nobjRigid = nobj.AddComponent<Rigidbody2D>();//On Mouse Down fonksiyonu çalışabilsin diye Rigidbody ekliyoruz.
                nobjRigid.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;//Fizikten kaynaklı pozisyonlarını donduruyoruz.

            }
        }
        
    }
    void createWall()//Yürünemez alan oluşturmak için kullandığımız fonksiyon.
    {
        for (int x =0; x < iwallCount; x++)
        {
            int iXrandom = (int)Random.Range(1f, 9f);//Duvarları random oluşturuyoruz.
            int iYrandom = (int)Random.Range(1f, 9f);
            Nodegrid[iXrandom, iYrandom].bIsBlocked = true;//Rastegele seçilen noktanın kontrol değerini true yapıyoruz.
            GameObject.Find(iXrandom + "," + iYrandom).GetComponent<Renderer>().material.color = Color.black;//Seçilen noktanın rengini siyah olarak değiştiriyoruz.
        }
    }

    public Node NodePosReturnNode(Vector2 a_Pos)//Pozisyonu verilen değerine karşılık gelen Kareyi dönen fonksiyon
    {

        int ixPos = Mathf.RoundToInt(a_Pos.x);//Pozisyonun x değerini int çeviriyor.
        int iyPos = Mathf.RoundToInt(a_Pos.y);//Pozisyonun y değerini int çeviriyor.
        return Nodegrid[ixPos, iyPos];//Bulunan x ve y değerlerine karşılık gelen düğümü döndürür.
    }


    public List<Node> NeighboringNodesReturn(Node a_NeighborNode)
    {
        List<Node> NeighborList = new List<Node>();//Make a new list of all available neighbors.
        int icheckX;//Variable to check if the XPosition is within range of the node array to avoid out of range errors.
        int icheckY;//Variable to check if the YPosition is within range of the node array to avoid out of range errors.

        //Check the right side of the current node.
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodegrid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Left side of the current node.
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY;
        if (icheckX >= 0 && icheckX < iGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodegrid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Top side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < iGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodegrid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        //Check the Bottom side of the current node.
        icheckX = a_NeighborNode.iGridX;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < iGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodegrid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        //Check the Bottom-left side of the current node
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < iGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodegrid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        //Check the Bottom-rigt side of the current node
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY - 1;
        if (icheckX >= 0 && icheckX < iGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodegrid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        //Check the Top-left side of the current node
        icheckX = a_NeighborNode.iGridX - 1;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < iGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodegrid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }

        //Check the Top-rigt side of the current node
        icheckX = a_NeighborNode.iGridX + 1;
        icheckY = a_NeighborNode.iGridY + 1;
        if (icheckX >= 0 && icheckX < iGridWidth)//If the XPosition is in range of the array
        {
            if (icheckY >= 0 && icheckY < iGridHeight)//If the YPosition is in range of the array
            {
                NeighborList.Add(Nodegrid[icheckX, icheckY]);//Add the grid to the available neighbors list
            }
        }
        return NeighborList;//Return the neighbors list.
    }




}
