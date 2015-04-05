using UnityEngine;
using System.Collections;

public class Calendar : MonoBehaviour 
{
	public float actualTime;
	public float gameTime;
	public float timeSpeed;

	public float dayTime;
	public int day;
	public int dayOfMonth;
	public float unitsInDay;

	public int month;
	public int monthOfYear;
	public int daysInMonth;

	public int year;
	public int monthsInYear;

	// Use this for initialization
	void Start () 
	{
		actualTime = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		actualTime = Time.time;
		gameTime += Time.deltaTime * timeSpeed;

		dayTime = gameTime % unitsInDay;
		day = (int) (gameTime / unitsInDay);
		dayOfMonth = (int) ((gameTime / unitsInDay) % daysInMonth);

		month = (int) (day / daysInMonth);
		monthOfYear = (int) ((day / daysInMonth) % monthsInYear);

		year = (int) (month / monthsInYear);
	}
}
