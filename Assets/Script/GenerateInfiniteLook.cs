using UnityEngine;

namespace Script
{
    public class GenerateInfiniteLook : MonoBehaviour
    {
        public GameObject empty;
        [Header("Harita Özellikleri")]
        [SerializeField]public GameObject mapPrefab;
        [SerializeField]public GameObject mapParent;
        
        [SerializeField]public int mapHeight;
        [SerializeField]public int mapWidth;
        [SerializeField]public int mapDepth;
        [Tooltip("Çift sayı girilmesi gerekiyor")]
        [SerializeField][Range(2, 4)] public int repeatValue=1;
        
        public void Generate()
        {
            for (int x = 0; x < repeatValue * 2 + 1; x++)
            {
                for (int y = 0; y < repeatValue * 2 + 1; y++)
                {
                    for (int z = 0; z < repeatValue * 2 + 1; z++)
                    {
                        GameObject map = Instantiate(mapPrefab, mapParent.transform);
                        map.transform.position = new Vector3(
                            (mapWidth * x)-(((repeatValue * 2 + 1) * mapWidth)/2), 
                            (mapHeight * y)-(((repeatValue * 2 + 1) * mapHeight)/2), 
                            (mapDepth * z)-(((repeatValue * 2 + 1) * mapDepth)/2));
                        
                        if (x == (repeatValue * 2 + 1) / 2 && 
                            y == (repeatValue * 2 + 1) / 2 &&
                            z == (repeatValue * 2 + 1) / 2)
                        {
                            GameObject collider = Instantiate(empty);
                            collider.transform.position=Vector3.zero;
                            collider.name = "Collider";

                            GameObject forward = Instantiate(empty);
                            forward.name = "Forward";
                            forward.tag = "Forward";
                            forward.transform.position = new Vector3(0, 0, mapDepth / 2);
                            forward.transform.parent = collider.transform;
                            BoxCollider boxColliderForward = forward.AddComponent<BoxCollider>();
                            boxColliderForward.isTrigger = true;
                            boxColliderForward.size = new Vector3(mapWidth,mapHeight,.5f);

                            GameObject back = Instantiate(empty);
                            back.name = "Back";
                            back.tag = "Back";
                            back.transform.position = new Vector3(0, 0, -mapDepth / 2);
                            back.transform.parent = collider.transform;
                            BoxCollider boxColliderBack = back.AddComponent<BoxCollider>();
                            boxColliderBack.isTrigger = true;
                            boxColliderBack.size = new Vector3(mapWidth,mapHeight,.5f);

                            GameObject right = Instantiate(empty);
                            right.name = "Right";
                            right.tag = "Right";
                            right.transform.position = new Vector3(mapWidth/2, 0, 0);
                            right.transform.parent = collider.transform;
                            BoxCollider boxColliderRight = right.AddComponent<BoxCollider>();
                            boxColliderRight.isTrigger = true;
                            boxColliderRight.size = new Vector3(.5f,mapHeight,mapDepth);

                            GameObject left = Instantiate(empty);
                            left.name = "Left";
                            left.tag = "Left";
                            left.transform.position = new Vector3(-mapWidth/2, 0, 0);
                            left.transform.parent = collider.transform;
                            BoxCollider boxColliderLeft = left.AddComponent<BoxCollider>();
                            boxColliderLeft.isTrigger = true;
                            boxColliderLeft.size = new Vector3(.5f,mapHeight,mapDepth);

                            GameObject up = Instantiate(empty);
                            up.name = "Up";
                            up.tag = "Up";
                            up.transform.position = new Vector3(0, mapHeight/2, 0);
                            up.transform.parent = collider.transform;
                            BoxCollider boxColliderUp = up.AddComponent<BoxCollider>();
                            boxColliderUp.isTrigger = true;
                            boxColliderUp.size = new Vector3(mapWidth,.5f,mapDepth);

                            GameObject down = Instantiate(empty);
                            down.name = "Down";
                            down.tag = "Down";
                            down.transform.position = new Vector3(0, -mapHeight/2, 0);
                            down.transform.parent = collider.transform;
                            BoxCollider boxColliderDown = down.AddComponent<BoxCollider>();
                            boxColliderDown.isTrigger = true;
                            boxColliderDown.size = new Vector3(mapWidth,.5f,mapDepth);
                            
                            collider.transform.position=map.transform.position;
                        }
                    }
                }
            }
            
        }
    }
}
