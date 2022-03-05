using UnityEngine;
using System.Collections.Generic;
public class SceneInitializer : MonoBehaviour {

	private int OutWallSize_X = -1;
	private int OutWallSize_Y;


	public int MAP_SIZE_X = 35;
	public int MAP_SIZE_Y = 35;


	public int MAX_ROOM_NUMBER = 12;
	public int MIN_ITEM = 0;
	public int MAX_ITEM = 5;

	public int MIN_ENEMY = 2;
	public int MAX_ENEMY = 10;

	public GameObject _player;
	public GameObject exit;
	public GameObject[] enemy;
	public GameObject[] item;
	public List<Position> itemPosition = new List<Position>();
	public Position exitPosition;
	public Position playerPosition;

	private GameObject floorPrefab;
	private GameObject wallPrefab;
	private GameObject outWallPrefab;


	private int[,] map;

	// Use this for initialization
	void Start ()
	{
		OutWallSize_Y = MAP_SIZE_Y;
		GenerateMap();
		SponePlayer();
		GenerateExit();
		GenerateItem();
		GenerateEnemy();
	}

	private void GenerateEnemy()
    {
		Position position;

		int generateRandomNumber = Random.Range(MIN_ENEMY, MAX_ENEMY); 

		for (int i = 0; i < generateRandomNumber; i++)
		{
			do
			{
				var x = RogueUtils.GetRandomInt(0, MAP_SIZE_X - 1);
				var y = RogueUtils.GetRandomInt(0, MAP_SIZE_Y - 1);
				position = new Position(x, y);
			}
			while (map[position.X, position.Y] != 1 && playerPosition != position && exitPosition != position); //生成したアイテムの座標と全て付け合わせる(0～4)　→確率的に低いのでとりあえず保留

			int random = Random.Range(0, enemy.Length);

			itemPosition.Add(new Position(position.X, position.Y));
			Instantiate(enemy[random], new Vector3(position.X, 0, position.Y), Quaternion.identity,this.transform);

		}

	}


	private void GenerateMap() {
		map = new MapGenerator().GenerateMap(MAP_SIZE_X, MAP_SIZE_Y, MAX_ROOM_NUMBER);

		string log = "";
		for (int y = 0; y < MAP_SIZE_Y; y++) {
			for (int x = 0; x < MAP_SIZE_X; x++) {
				log += map[x, y] == 0 ? " " : "1";
			}
			log += "\n";
		}
		Debug.Log(log);

		floorPrefab = Resources.Load("Prefabs/Floor") as GameObject;
		wallPrefab = Resources.Load("Prefabs/Wall") as GameObject;
		outWallPrefab = Resources.Load("Prefabs/OutWall") as GameObject;

		var floorList = new List<Vector3>();
		var wallList = new List<Vector3>();

		for (int y = 0; y < MAP_SIZE_Y; y++) {
			for (int x = 0; x < MAP_SIZE_X; x++) {
				if (map[x, y] == 1) {
					var a=Instantiate(floorPrefab, new Vector3(x, 0, y), new Quaternion(), this.transform);
				} else {
					Instantiate(wallPrefab, new Vector3(x, 0, y), new Quaternion(), this.transform);
					
				}
			}
		}

		for (int x = OutWallSize_X; x < OutWallSize_Y; x++)
		{
			Instantiate(outWallPrefab, new Vector3(x, 0, OutWallSize_X), Quaternion.identity,this.transform);
			Instantiate(outWallPrefab, new Vector3(x, 0, OutWallSize_Y), Quaternion.identity, this.transform);
		}

		for(int y=OutWallSize_X;y<OutWallSize_Y;y++)
        {
			Instantiate(outWallPrefab, new Vector3(OutWallSize_X,0, y), Quaternion.identity, this.transform);
			Instantiate(outWallPrefab, new Vector3(OutWallSize_Y, 0, y), Quaternion.identity, this.transform);
        }

	}

	public void SponePlayer() 
	{
		Position position;

		do
		{
			var x = RogueUtils.GetRandomInt(0, 	MAP_SIZE_X - 1);
			var y = RogueUtils.GetRandomInt(0, 	MAP_SIZE_Y - 1);
			position = new Position(x, y);
		}
		while (map[position.X, position.Y] != 1);

		playerPosition = new Position(position.X, position.Y);
		Debug.Log(_player.transform.position);

		Instantiate(_player, new Vector3(position.X, 0, position.Y), Quaternion.identity);

	}

	public void GenerateExit()
	{

		Position position;

		do
		{
			var x = RogueUtils.GetRandomInt(0, MAP_SIZE_X - 1);
			var y = RogueUtils.GetRandomInt(0, MAP_SIZE_Y - 1);
			position = new Position(x, y);
		}
		while (map[position.X, position.Y] != 1 && playerPosition!=position); //これでplayerとExitの座標の付け合わせできてる。

		Instantiate(exit, new Vector3(position.X, 0, position.Y), Quaternion.identity, this.transform);
		exitPosition = new Position(position.X, position.Y);

	}

	private void GenerateItem()
	{
		Position position;

		int generateRandomNumber = Random.Range(MIN_ITEM, MAX_ITEM); //アイテムを下限～上限の間の数で生成

		for(int i=0;i<generateRandomNumber;i++)
        {
			do
			{
				var x = RogueUtils.GetRandomInt(0, MAP_SIZE_X - 1);
				var y = RogueUtils.GetRandomInt(0, MAP_SIZE_Y - 1);
				position = new Position(x, y);
			}
			while (map[position.X, position.Y] != 1 && playerPosition!=position　&& exitPosition!=position); //生成したアイテムの座標と全て付け合わせる(0～4)　→確率的に低いのでとりあえず保留

			int random = Random.Range(0, item.Length);

			itemPosition.Add(new Position(position.X, position.Y));
			var a=Instantiate(item[random], new Vector3(position.X, 0, position.Y), Quaternion.identity, this.transform);
			a.name = item[random].name;

		}

	}

}
