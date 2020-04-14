using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class that controlls character movement/actions
public class PlayerThirdPersonInput : MonoBehaviour
{
    //Object that manages unit movement
    UnitController unitController;
    //Object that manages weapons
    WeaponController weaponController;

    //Vars used to raycast pointer direction
    RaycastHit hitInfo = new RaycastHit();
    Ray ray = new Ray();

    //C is for no movement
    enum Direction { C, N, NE, E, SE, S, SW, W, NW };

    //Direction mapping to vectors
    Dictionary<Direction, Vector3> directions = new Dictionary<Direction, Vector3>
    {
        [Direction.C] = Vector3.zero,
        [Direction.N] = Vector3.forward,
        [Direction.NE] = (Vector3.forward + Vector3.right) / 1.41f,
        [Direction.E] = Vector3.right,
        [Direction.SE] = (-Vector3.forward + Vector3.right) / 1.41f,
        [Direction.S] = -Vector3.forward,
        [Direction.SW] = (-Vector3.forward - Vector3.right) / 1.41f,
        [Direction.W] = -Vector3.right,
        [Direction.NW] = (Vector3.forward - Vector3.right) / 1.41f,
    };
    Direction dir = Direction.C;
    [SerializeField]
    string[] stepableLayers;
    int layerMask;

    private void Start()
    {
        unitController = GetComponentInChildren<UnitController>();
        weaponController = GetComponentInChildren<WeaponController>();
        layerMask = LayerMask.GetMask(stepableLayers);
        Debug.Log(layerMask);
    }

    //U: Check user inputs and execute player actions, only works if player control is active
    void Update()
    {
        Move();
        LookAt();
        WeaponAction();
    }

    //U: Returns the direction the player should move depending on user input
    private Direction GetDirectionInput()
    {
        Direction result = Direction.C;

        //U: Checks user input
        bool upKey = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W));
        bool rightKey = (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D));
        bool downKey = (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S));
        bool leftKey = (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A));

        //U: If two incompatible keys are pressed they neutralize each other
        if (downKey && upKey)
        {
            downKey = false;
            leftKey = false;
        }
        if (rightKey && leftKey)
        {
            rightKey = false;
            leftKey = false;
        }

        //U: First check for diagonals moves, then for axis movement
        if (upKey && rightKey)
        {
            result = Direction.NE;
        }
        else if (downKey && rightKey)
        {
            result = Direction.SE;
        }
        else if (downKey && leftKey)
        {
            result = Direction.SW;
        }
        else if (upKey && leftKey)
        {
            result = Direction.NW;
        }
        else if (upKey)
        {
            result = Direction.N;
        }
        else if (rightKey)
        {
            result = Direction.E;
        }
        else if (downKey)
        {
            result = Direction.S;
        }
        else if (leftKey)
        {
            result = Direction.W;
        }

        return result;
    }

    //U: Moves the player around
    private void Move()
    {
        dir = GetDirectionInput();
        unitController.SetMovDir(directions[dir]);
    }

    //This objects are created here to avoid instantiating every frame
    
    //U: Rotates the player towards the aim
    private void LookAt()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hitInfo, 200f, layerMask))
        {
            Debug.Log(hitInfo.point);
            Vector3 origin = weaponController.transform.position;
            Vector3 offset = Vector3.up * weaponController.transform.localPosition.y;
            Vector3 hitPoint = hitInfo.point + offset;

            unitController.LookAt(hitPoint);
            weaponController.PointAt(hitPoint);
        }
    }

    //U: Shoots or change weapon if commanded
    private void WeaponAction()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weaponController.SwitchWeapon(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weaponController.SwitchWeapon(1);
        }
        if (Input.GetMouseButton(0))
        {
            weaponController.TryAction();
        }
    }

    

}
