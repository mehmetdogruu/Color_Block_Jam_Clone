using UnityEngine;
using DG.Tweening;
public class GateDetectorController : MonoBehaviour
{
    public IdleState IdleState;
    public CheckState CheckState;
    public VacuumState VacuumState;


    private IState currentState; 


    [SerializeField] private BoxCollider detectorCollider;
    [SerializeField] private BoxCollider mainCollider;
    [SerializeField] private GateDetectorsTriggerController detectorsTriggerController;
    private FrameController frameController;
    [SerializeField] private ParticleSystem collisionParticle;
    [SerializeField] private GameObject model;


    public bool detectorColliderTriggered = false;

    public BlockController vacuumedObject;
    private void Awake()
    {
        frameController = GetComponentInParent<FrameController>();

        IdleState = new IdleState(this);
        CheckState = new CheckState(this);
        VacuumState = new VacuumState(this);

    }
    protected virtual void Start()
    {

        ChangeState(CheckState); 
    }
    protected virtual void Update()
    {
        currentState?.UpdateState();
    }
    public void ChangeState(IState newState)
    {
        currentState?.ExitState();

        currentState = newState;
        currentState.EnterState();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            detectorColliderTriggered = true;
            Debug.Log("detectorCollider bir objeyle etkileþime girdi: " + other.name);
            vacuumedObject = other.transform.parent.gameObject.GetComponentInParent<BlockController>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other != null)
        {
            detectorColliderTriggered = false;
            Debug.Log("detectorCollider objeden çýktý: " + other.name);
            vacuumedObject = null;
        }
    }
    public bool CheckObjectForVacuum()
    {
        if (detectorsTriggerController.isColTriggered == false  && detectorColliderTriggered == true && vacuumedObject != null && vacuumedObject.Team==frameController.Team)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void VacuumObject()
    {

        if (CheckObjectForVacuum())
        {
            DisableBlocksDrag();
            Debug.Log("vakumlama");
            mainCollider.isTrigger = true;
            vacuumedObject.SetDraggable = false;
            collisionParticle.Play();

            if (vacuumedObject.TryGetComponent<MultipleBlockController>(out var multipleBlockController))
            {
                multipleBlockController.ShowInsideObject();
            }
            vacuumedObject.transform.DOScale(0, 1f).OnStart(() =>
            {
                vacuumedObject.transform.DOMove(transform.position, 1f);
                vacuumedObject.Col.SetActive(false);
                model.transform.DOLocalRotate(Vector3.right * 360, 1f, RotateMode.LocalAxisAdd);


            }).OnComplete(() =>
            {
                BlockManager.Instance.blocks.Remove(vacuumedObject);
                EnableBlocksDrag();

   
                vacuumedObject = null;
                mainCollider.isTrigger = false;
                ChangeState(CheckState);



            });
        }
    }
     private void DisableBlocksDrag()
    {
        foreach (var item in BlockManager.Instance.blocks)
        {
            item.SetDraggable = false;
        }
    }
    private void EnableBlocksDrag()
    {
        foreach (var item in BlockManager.Instance.blocks)
        {
            item.SetDraggable = true;
        }
    }
    public void CheckLevelCompleted()
    {
        if (BlockManager.Instance.blocks.Count == 0)
        {
            UIManager.Instance.ShowWinPanel();
            CurrencyManager.Instance.AddCoin(50);
            UIManager.Instance.UpdateText(CurrencyManager.Instance.GetCoinAmount());
            TimeManager.Instance.timerActive=false;
            UIManager.Instance.StartCoinAnimation();
        }
    }

}
