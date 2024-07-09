using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class TowerManager : Loader<TowerManager>
{
    public TowerBtn towerBtnPressed {  get; set; }
    //public static object Instantance { get; internal set; }

    SpriteRenderer spriteRenderer;

    private List<TowerControl> TowersList = new List<TowerControl> ();
    private List<Collider2D> BuildList = new List<Collider2D> ();
    private Collider2D buildTile;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        buildTile = GetComponent<Collider2D> ();
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

            if (hit.collider.tag == "TowerSide")
            {
                buildTile = hit.collider;
                buildTile.tag = "TowerSideFull";
                RegisterBuildSide(buildTile);
                PlaceTower(hit);
            }

        }
        if (spriteRenderer.enabled)
        {
            followMouse();
        }
    }

    public void RegisterBuildSide (Collider2D buildTag)
    {
        BuildList.Add(buildTag);
    }

    public void RegisterTower (TowerControl tower)
    {
        TowersList.Add(tower);
    }

    public void RenameTagBuildSide()
    {
        foreach (Collider2D buildTag in BuildList)
        {
            buildTag.tag = "TowerSide";
        }
        BuildList.Clear();
    }

    public void DestroyAllTowers()
    {
        foreach(TowerControl tower in TowersList)
        {
            Destroy(tower.gameObject);
        }
        TowersList.Clear();
    }
    public void PlaceTower(RaycastHit2D hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && towerBtnPressed != null)
        {
            TowerControl newTower = Instantiate(towerBtnPressed.TowerObject);
            newTower.transform.position = hit.transform.position;
            BuyTower(towerBtnPressed.TowerPrice);
            Manager.Instance.AudioSource.PlayOneShot(SoundManager.Instance.TowerBuilt);
            RegisterTower(newTower);
            DisableDrag();

        }
    }

    public void BuyTower(int price)
    {
        Manager.Instance.subtracMoney(price);
    }
    public void SelectedTower(TowerBtn towerSelected)
    {
        if(towerSelected.TowerPrice <= Manager.Instance.TotalMoney)
        {
            towerBtnPressed = towerSelected;
            EnableDrag(towerBtnPressed.DragSprite);
        }
        /*towerBtnPressed = towerSelected;
        EnableDrag(towerBtnPressed.DragSprite);
        Debug.Log("Pressed" + towerBtnPressed.gameObject);*/
    }

    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void EnableDrag(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void DisableDrag()
    {
        spriteRenderer.enabled = false;
    }
}


