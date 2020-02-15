using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    Grid GridReference;

    [Header("Transform")]
    public Transform tStartPos;
    public Transform tTargetPos;

    [Header("Node")]
    Node nStartNode;
    Node nTargetNode;

    bool findedPath;

    List<Node> lnCalculatedList;
    HashSet<Node> lnNotCalculatedList;

    private void Awake()
    {
        GridReference = GetComponent<Grid>();
    }

    public float GvalueReturn(Node currentNode,Node neighborNode)
    {
        currentNode.vPosition = new Vector2(currentNode.iGridX, currentNode.iGridY);
        neighborNode.vPosition = new Vector2(neighborNode.iGridX, neighborNode.iGridY);

        currentNode.iGvalue = Vector2.Distance(currentNode.vPosition, neighborNode.vPosition);

        return currentNode.iGvalue;
    }
    public float HvalueReturn(Node currentNode,Node targetNode)
    {
        currentNode.vPosition = new Vector2(currentNode.iGridX, currentNode.iGridY);
        targetNode.vPosition = new Vector2(targetNode.iGridX, targetNode.iGridY);

        currentNode.iHvalue = Vector2.Distance(currentNode.vPosition, targetNode.vPosition);

        return currentNode.iHvalue;
    }

    void FindPath(Vector2 a_StartPos, Vector2 a_TargetPos)
    {
        nStartNode  = GridReference.NodePosReturnNode(a_StartPos);
        nTargetNode = GridReference.NodePosReturnNode(a_TargetPos);

        lnCalculatedList = new List<Node>();
        lnNotCalculatedList = new HashSet<Node>();
        
        lnCalculatedList.Add(nStartNode);

        while (lnCalculatedList.Count > 0)
        {
            Node nCurrentNode = lnCalculatedList[0];

            for (int i = 1; i < lnCalculatedList.Count; i++)
            {
                    if (lnCalculatedList[i].fvalue < nCurrentNode.fvalue || lnCalculatedList[i].fvalue == nCurrentNode.fvalue && lnCalculatedList[i].iHvalue < nCurrentNode.iHvalue)
                    {
                        nCurrentNode = lnCalculatedList[i];
                    }
            }

            lnCalculatedList.Remove(nCurrentNode);
            lnNotCalculatedList.Add(nCurrentNode);

            if (nCurrentNode == nTargetNode)
            {
                findedPath = true;
                PathReturn(nStartNode, nTargetNode);
            }
            foreach (Node NeighborNode in GridReference.NeighboringNodesReturn(nCurrentNode))
            {
                if (NeighborNode.bIsBlocked || lnNotCalculatedList.Contains(NeighborNode))
                {
                    continue;
                }
                float fNewPath = nCurrentNode.iGvalue + HvalueReturn(nCurrentNode, NeighborNode);

                if(fNewPath < NeighborNode.iGvalue || !lnCalculatedList.Contains(NeighborNode))
                {
                    NeighborNode.iGvalue = fNewPath;
                    NeighborNode.iHvalue = HvalueReturn(NeighborNode, nTargetNode);
                    NeighborNode.ParentNode = nCurrentNode;

                    if (!lnCalculatedList.Contains(NeighborNode))
                    {
                        lnCalculatedList.Add(NeighborNode);
                    }
                }
            }


        }
    }

    void PathReturn(Node startNode,Node targetNode)
    {
        List<Node> nPath = new List<Node>();
        Node CurrentNode = targetNode;

        while(CurrentNode != startNode)
        {
            nPath.Add(CurrentNode);
            CurrentNode = CurrentNode.ParentNode;
        }

        nPath.Reverse();

        Color32 color = new Color32(144, 0, 255, 255);
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("box"))
        {
            if (g.GetComponent<Renderer>().material.color == color)
                g.GetComponent<Renderer>().material.color = Color.white;
        }
        foreach (Node x in nPath)
        {
            GameObject.Find(x.iGridX + "," + x.iGridY).GetComponent<Renderer>().material.color = color;
        }
    }
   public void MousePos(int x,int y,int z)
    {
        if (Input.GetMouseButtonDown(0))
        {
            findedPath = false;
            if (!GridReference.Nodegrid[x,y].bIsBlocked)
            {
                foreach (GameObject g in GameObject.FindGameObjectsWithTag("box"))
                {
                    Color32 color = new Color32(144, 0, 255, 255);
                    if (g.GetComponent<Renderer>().material.color == color)
                        g.GetComponent<Renderer>().material.color = Color.white;
                }
                GridReference.gTarget.gameObject.transform.position = new Vector3(x, y, z);
                FindPath(tStartPos.position, tTargetPos.position);
                if (findedPath)
                {
                    GridReference.gTarget.gameObject.transform.position = new Vector3(x, y, -9);
                }
                else
                {
                    print("yol bulunamadı");
                }
                
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            if (!GridReference.Nodegrid[x, y].bIsBlocked)
            {
                GridReference.gHero.gameObject.transform.position = new Vector3(x, y, z);
                FindPath(tStartPos.position, tTargetPos.position);
            }
        }
    }

}
