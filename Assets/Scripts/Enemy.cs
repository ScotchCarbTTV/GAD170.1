using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;

public class Enemy : MonoBehaviour
{
    //reference to the nav agent
    [SerializeField] NavMeshAgent agent;

    //reference to the player
    [SerializeField] GameObject player;

    //reference to the patrol node
    [SerializeField] GameObject patrolNode;

    [SerializeField] Vector2 patrolArea;

    [SerializeField] GameObject coin;

    //create StateMachine variable
    public StateMachine StateMachine { get; private set; }

    private Transform target;

    
    

    private void Awake()
    {
        //declare new StateMachine and assign it to the variable
        StateMachine = new StateMachine();
        patrolArea = new Vector2();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //set the initial state of the StateMachine
        StateMachine.SetState(new EnemyPatrol(this));
        if(patrolNode != null) 
        { 
            patrolArea = new Vector2(patrolNode.transform.position.x, patrolNode.transform.position.z);
        }
    }

    private void Update()
    {
        //move towards current target
        agent.SetDestination(target.position);
        StateMachine.OnUpdate();
    }

    //abstract class for defining the enemy states which inherits from the state machine
    public abstract class EnemyState : IState
    {
        protected Enemy instance;

        //constructor for the enemy states
        public EnemyState(Enemy _instance)
        {
            instance = _instance;
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {            
        }

        public virtual void OnUpdate()
        {
        }
    }

    public void Chase()
    {
        StateMachine.SetState(new EnemyChase(this));
    }
    public void Patrol()
    {
        StateMachine.SetState(new EnemyPatrol(this));
    }

    public class EnemyPatrol : EnemyState
    {
        public EnemyPatrol(Enemy _instance) : base(_instance)
        { }
        public override void OnEnter()
        {
            //set the follow target to the nav 'empty'
            instance.target = instance.patrolNode.transform;
        }

        public override void OnUpdate()
        {
            if (Vector3.Distance(instance.transform.position, instance.patrolNode.transform.position) < 1)
            {
                instance.patrolNode.transform.position = new Vector3(Random.Range(instance.patrolArea.x - 5, instance.patrolArea.x + 5), instance.patrolNode.transform.position.y, Random.Range(instance.patrolArea.y - 5, instance.patrolArea.y + 5));
            }
            else if (Vector3.Distance(instance.transform.position, instance.player.transform.position) < 8)
            {
                instance.Chase();
            }
        }
    }

    public class EnemyChase : EnemyState
    {
        public EnemyChase(Enemy _instance) : base(_instance)
        { }
        public override void OnEnter()
        {
            //set the follow target to the player
            instance.target = instance.player.transform;
        }

        public override void OnUpdate()
        {
            if (Vector3.Distance(instance.transform.position, instance.player.transform.position) > 8)
            {
                instance.Patrol();
            }
        }
    }   

    public void Death()
    {
        Instantiate(coin, new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z +1), transform.rotation);
        Instantiate(coin, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), transform.rotation);
        Instantiate(coin, new Vector3(transform.position.x -1, transform.position.y + 1, transform.position.z -1), transform.rotation);
        gameObject.SetActive(false);
    }

}



