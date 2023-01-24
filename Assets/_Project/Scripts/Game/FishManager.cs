//using UnityEngine;

//public class FishManager : MonoBehaviour
//{
//    [HideInInspector] public Transform active;

//    private bool moving;
//    private Vector3 mousePos;

//    private void Update()
//    {
//        if (active == null) return;

//        mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y);

//        if (moving) active.position = mousePos;

//        if (Input.GetMouseButtonDown(0))
//        {
//            if (active.GetComponent<BoxCollider2D>().bounds.Contains(mousePos)) moving = true;
//        }

//        if (Input.GetMouseButtonUp(0)) moving = false;
//    }
//}