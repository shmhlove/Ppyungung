using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombEffectManager : MonoBehaviour 
{
	public SpriteRenderer m_pRenderer = null;
	public Sprite [] m_pSprite = null;

	private string m_strBaseName = "fire_sprite_01";
	public float iAnimCount = 0.0f;


	private int iCurrentCount = 0;

	// Use this for initialization
	void Update () 
	{
		if (iCurrentCount == (int)iAnimCount)
			return;
		
		iCurrentCount = (int)iAnimCount;
		if (iCurrentCount >= m_pSprite.Length)
			return;
		
		m_pRenderer.sprite = m_pSprite[iCurrentCount];
	}
}
