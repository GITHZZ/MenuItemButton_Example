using UnityEngine;
using System.Collections;

enum ButtonState
{
	unselected = 0,
	selected = 1,
	disable = 2
}

public class MenuItemButton : MonoBehaviour 
{
	public Texture2D unselect;
	public Texture2D select;
	
	public string selector_func_name;

	private float scale;
	private SpriteRenderer m_spriteRender;
	private ButtonState m_state;

	private Sprite unSelectSprite;
	private Sprite selectSprite;

	private Camera mainCamera;

	private float currentTime;
	private float intervalTime;

	void Awake ()
	{
		m_spriteRender = this.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
		m_state = ButtonState.unselected;

		GameObject cameraObj = GameObject.FindWithTag("MainCamera");		
		mainCamera = cameraObj.GetComponent(typeof(Camera)) as Camera;
		
		Vector2 btnPos2D = mainCamera.WorldToScreenPoint(this.transform.position);

		Rect unSelectRect = new Rect(0, 0, unselect.width, unselect.height);
		unSelectSprite = Sprite.Create(unselect, unSelectRect, new Vector2(0.5f, 0.5f));

		Rect selectRect = new Rect(0, 0, select.width, select.height);
		selectSprite = Sprite.Create(select, selectRect, new Vector2(0.5f, 0.5f));
	}

	void Start () 
	{
		currentTime = 0.0f;
		intervalTime = 0.01f;

		if(m_spriteRender.sprite == null)
		{
			m_spriteRender.sprite = unSelectSprite as Sprite;
		}
	}
	
	void Update () 
	{
		currentTime += Time.deltaTime;
		if(currentTime > intervalTime)
		{
			currentTime = 0.0f;
			
			if(Input.GetMouseButtonDown(0) && m_state == ButtonState.unselected)
			{
				//点击按下操作
				Vector2 inputPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				Vector2 btnPos2D = mainCamera.WorldToScreenPoint(this.transform.position);
				Rect unSelectRect = new Rect(btnPos2D.x - unselect.width/2,
											 btnPos2D.y - unselect.height/2, 
											 unselect.width, 
											 unselect.height);
	
				if(unSelectRect.Contains(inputPos))
				{
					m_state = ButtonState.selected;
					m_spriteRender.sprite = selectSprite as Sprite;
				}	
			}
			else if(Input.GetMouseButtonUp(0) && m_state == ButtonState.selected)
			{
				//点击抬起操作
				m_state = ButtonState.unselected;
				m_spriteRender.sprite = unSelectSprite as Sprite;

				Vector2 inputPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				Vector2 btnPos2D = mainCamera.WorldToScreenPoint(this.transform.position);
				Rect selectRect = new Rect(btnPos2D.x - select.width/2, 
										   btnPos2D.y - select.height/2, 
										   select.width, 
										   select.height);
	
				if(selectRect.Contains(inputPos))
				{
					menuButton_Func();
				}
			}
		}
	}

	void menuButton_Func()
	{
		if(selector_func_name != "")
		{
			this.SendMessage(selector_func_name);
		}
	}
}
