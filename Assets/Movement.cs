using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Movement : MonoBehaviour
{
    private NavMeshAgent man;
    // RaycastHit hit = new RaycastHit();
    bool Attack;
    float AttackCd;

    public GameObject currentTarget;
    private GameObject myBullet;
    public bool CanCreate;
    public float targetSize;

    public int Status;
    //1 = rest; 2 = attact; 3 = move

    public List<GameObject> theGroupList;
    Vector3 previousPosition;
    float prevousSpeed;
    public bool isCohesion = true;

    Vector3 SavingPos;

    // add game controller here
    public GameObject gc;

    // Start is called before the first frame update
    void Start()
    {
        man = GetComponent<NavMeshAgent>();
        Attack = false;
        currentTarget = null;
        CanCreate = true;
        targetSize = 6.9f;
        Status = 1;
        prevousSpeed = 0;
        AttackCd = 1 / (float)GetComponent<Unit_info>().attSpeed;
        SavingPos = transform.position;

        gc = GameObject.Find("GameController").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Unit_info>().ID >= 0 && GetComponent<Unit_info>().ID < 10) return;
        if (currentTarget != null)
        {
            AttackRanger(currentTarget);
            if (Attack)
            {
                if (GetComponent<NavMeshAgent>() != null)
                {
                    man.destination = transform.position;
                }
                else
                {
                    //SavingPos = transform.position;
                }
                Status = 2;
            }
        }

        StatusCheck();
        switch (Status)
        {
            case 1:
                GetComponent<Unit_info>().SetState(Unit_info.State.Idle);
                break;
            case 2:
                // Farmer
                if (GetComponent<Unit_info>().ID == 10)
                {
                    GetComponent<Unit_info>().SetState(Unit_info.State.Working);
                }
                else
                {
                    GetComponent<Unit_info>().SetState(Unit_info.State.Attacking);
                }
                break;
            default:
                GetComponent<Unit_info>().SetState(Unit_info.State.Moving);
                break;
        }

        previousPosition = transform.position;
        AttackCd -= Time.deltaTime;

    }

    public void RemoveCompantAgent()
    {
        if (GetComponent<NavMeshAgent>() != null)
        {
            Destroy(GetComponent<NavMeshAgent>());
        }

    }

    public void StatusCheck()
    {
        Vector3 curMove = transform.position - previousPosition;
        float curSpeed = curMove.magnitude / Time.deltaTime;
        switch (Status)
        {
            case 1:
                //rest

                if (prevousSpeed < curSpeed && curSpeed > 0.5f)
                {
                    Status = 3;

                    GetComponent<Unit_info>().SetState(Unit_info.State.Moving);
                }
                if (GetComponent<NavMeshAgent>() != null)
                {
                    man.destination = transform.position;
                }
                ResetPosInList(theGroupList, 10);
                AttackNear();
                break;
            case 2:
                //attack
                if (currentTarget == null)
                {
                    Status = 1;
                }
                else
                {
                    AttackRanger(currentTarget);
                    DoAttack(currentTarget);
                    if (!CanCreate)
                    {
                        Status = 1;
                        CheckHit(currentTarget);
                    }
                    
                    if (!Attack)
                    {
                        currentTarget = null;
                        Status = 1;
                    }
                    

                }
                

                break;
            case 3:
                MovingUpdate();
                float SpeedCOunt = prevousSpeed - curSpeed;
                if (SpeedCOunt > -0.00001f && curSpeed < 1.1f && curSpeed > 0.00001f)
                {
                    if (GetComponent<NavMeshAgent>() != null)
                    {
                        man.destination = transform.position;
                    }
                    Status = 1;
                    GetComponent<Unit_info>().SetState(Unit_info.State.Idle);
                    curSpeed = 0;
                }
                prevousSpeed = curSpeed;
                ResetPosInList(theGroupList, 10);

                if (currentTarget == null) {
                   // DoNoAction();
                }

                if (GetComponent<Unit_info>().belongTo > 0)
                {
                    if (GetComponent<Unit_info>().ID > 10)
                    {
                        AttackNear();
                    }
                    
                }

                //move
                break;
            default:
                break;
        }
    }
    public void DoNoAction()
    {
        currentTarget = null;
        Attack = false;
        Status = 1;
    }

    public void MoveTo(Vector3 pos)
    {
        if (GetComponent<Unit_info>().ID == 14)
        {
            PlaneMove(pos);
        }
        else
        {
            GroundMove(pos);

        }
    }
    public void GroundMove(Vector3 pos)
    {
        currentTarget = null;
        man.destination = pos;
        Status = 3;
    }
    public void PlaneMove(Vector3 pos)
    {
        Vector3 newPos = pos;
        newPos.y = 20;
        SavingPos = newPos;
        currentTarget = null;
        //man.destination = pos;
        float step = (float)GetComponent<Unit_info>().moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, pos, step);
        Status = 3;
    }

    public void MovingUpdate()
    {
        if (GetComponent<Unit_info>().ID == 14)
        {
            float step = (float)GetComponent<Unit_info>().moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, SavingPos, step);

            Vector3 a, b, c, d;
            a = transform.position - SavingPos;
            b = transform.forward;
            float theta = Mathf.Abs(Mathf.Acos(Vector3.Dot(a, b) / a.magnitude / b.magnitude) / Mathf.PI);
            if (theta <= 0.9)
            {
                transform.LookAt(SavingPos);
                c = new Vector3(0, 1, 0);
                d = new Vector3(0, 0, 1);
                //transform.RotateAround(transform.position,c,180);
                transform.Rotate(90.0f, 0.0f, 0.0f, Space.Self);
                transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                transform.Rotate(c, Space.Self);
                transform.Rotate(d, Space.Self);
            }
            //transform.rotation = Quaternion.LookRotation(transform.forward, SavingPos);
            //transform.position = Vector3.RotateTowards(transform.forward, SavingPos, 0.01f, 0.01f);
        }
    }
    public void AttactTo(GameObject target)
    {

        currentTarget = target;
        AttackRanger(currentTarget);
        if (Attack)
        {
            if (GetComponent<Unit_info>().ID == 14)
            {
                SavingPos = transform.position;
                SavingPos.y = 20;
            }
            else
            {
                man.destination = transform.position;

            }
            Status = 2;
        }
        else
        {
            if (GetComponent<Unit_info>().ID == 14)
            {
                SavingPos = target.transform.position;
                SavingPos.y = 20;
            }
            else
            {
                man.destination = target.transform.position;

            }

            Status = 3;
        }


    }

    void AttackRanger(GameObject target)
    {
        var dist = Vector3.Distance(target.transform.position, transform.position);
        if (dist <= GetComponent<Unit_info>().attRange + target.GetComponent<Unit_info>().size)
        {
            Attack = true;
        }
        else
        {
            Attack = false;
        }
    }

    void DoAttack(GameObject target)
    {

        if (Attack && AttackCd <= 0)
        {
            if (GetComponent<Unit_info>().isRanger)
            {

                CreateBullet(target);
            }
            
            else if (target.GetComponent<Unit_info>().ID == 50)
            {
                gc.GetComponent<GameControl>().players[GetComponent<Unit_info>().belongTo].PaperNum += GetComponent<Unit_info>().attDmg-3;
            }
            else if (target.GetComponent<Unit_info>().ID == 51)
            {
                gc.GetComponent<GameControl>().players[GetComponent<Unit_info>().belongTo].GlueNum += GetComponent<Unit_info>().attDmg-4;

            }
            else
            {
                target.GetComponent<Unit_info>().hp -= GetComponent<Unit_info>().attDmg;
            }



            // rotate to target
            // except plane
            if (GetComponent<Unit_info>().ID == 14) {
                return;
            }
            Vector3 a = transform.position - target.transform.position;
            Vector3 b = transform.forward;
            float theta = Mathf.Abs(Mathf.Acos(Vector3.Dot(a, b) / a.magnitude / b.magnitude) / Mathf.PI);
            if (theta <= 0.8)
            {
                transform.LookAt(target.transform.position);
            }


            AttackCd = 1 / (float)GetComponent<Unit_info>().attSpeed;
        }
    }

    void CreateBullet(GameObject target)
    {
        if (CanCreate)
        {
            myBullet = (GameObject)Instantiate(Resources.Load("Models/Bullets/Farmer_Bullet"));
            myBullet.transform.position = transform.position + transform.forward * GetComponent<Unit_info>().size + transform.up * GetComponent<Unit_info>().size;
            myBullet.GetComponent<Bullet_script>().dmg = GetComponent<Unit_info>().attDmg;
            myBullet.GetComponent<Bullet_script>().target = target;
            CanCreate = false;
            // play particle
            if (transform.Find("ParticleSystem") != null)
            {
                transform.Find("ParticleSystem").GetComponent<ParticleSystem>().Play();
            }
        }
    }

    void CheckHit(GameObject target)
    {
        if (myBullet == null)
        {
            CanCreate = true;
        }
    }

    public void ResetPosInList(List<GameObject> theList, int howFar)
    {
        if (theList != null)
        {
            for (int i = 0; i < theList.Count; i++)
            {
                if (theList[i] == transform.gameObject)
                {
                }
                else
                {
                    /*
                    var dist = Vector3.Distance(theList[i].transform.position, transform.position);
                    if (dist > 2 && isCohesion)
                    {
                        MoveToFrom(theList[i]);
                        // break;
                    }
                    else if (dist < 10 && !isCohesion)
                    {
                        MoveAwayFrom(theList[i]);
                    }
                    else
                    {
                        //MoveAwayFrom(theList[i]);
                    }
                    */
                }
            }
        }

    }

    public void MoveToFrom(GameObject target)
    {
        //man.destination = transform.position;
        float step = (float)GetComponent<Unit_info>().moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
    }

    public void MoveAwayFrom(GameObject target)
    {
        float step = (float)GetComponent<Unit_info>().moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, -step);
    }

    public float SearchingR = 55;
    public void AttackNear() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, SearchingR);
        if (colliders.Length <= 0) {
            return;
        }

        for (int i = colliders.Length-1; i >=0; i--) {
            if (colliders[i].gameObject.GetComponent<Unit_info>() != null)
            {
                //&& GetComponent<Unit_info>().type ==Unit_info.Type.Building && GetComponent<Unit_info>().type == Unit_info.Type.Character
                if (colliders[i].gameObject.GetComponent<Unit_info>().belongTo != GetComponent<Unit_info>().belongTo && colliders[i].gameObject.GetComponent<Unit_info>().belongTo != -1)
                {
                    if (GetComponent<Unit_info>().ID > 10)
                    {
                        AttactTo(colliders[i].gameObject);
                    }

                }
            }
        }
        
            

            
        
    }
}
