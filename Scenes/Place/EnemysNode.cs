using Game.Objects.Entitys;
using Godot;
using Godot.Collections;
using Game.Managers;

public partial class EnemysNode : Node2D
{
	[ExportGroup("Spawn Settings")]
	[Export] private bool Enable = true;
	[Export] private float SpawnDelay = 10;
	[Export] private Array<PackedScene> Enemys;
	private Array<Vector2> SpawnPositions;

	private CharacterBody2D player;

	private Timer timer;

	private RandomNumberGenerator randomNumberGenerator;

	private GameManager gameManager;

	public override void _Ready()
	{
		randomNumberGenerator = new RandomNumberGenerator();

		player = GetParent().GetNodeOrNull("Player") as CharacterBody2D;

		gameManager = GetNodeOrNull("/root/GameManager") as GameManager;

		timer = new Timer();
		timer.WaitTime = SpawnDelay;
		timer.Autostart = true;
		timer.OneShot = true;
		AddChild(timer);

		timer.Timeout += OnTimerTimeout;

		timer.Name = "spawnTimer";

		SpawnPositions = new Array<Vector2>
		{
			new Vector2(-400, -200), 
			new Vector2(400, -200), 
			new Vector2(-400, 200),
			new Vector2(400, 200),
			new Vector2(0, 200),
			new Vector2(0, -200),
			new Vector2(400, 0),
			new Vector2(-400, 0)
		};
	}

	public override void _PhysicsProcess(double delta)
	{
		//gameManager.EnemysCount = GetChildCount() - 1;
		if (Input.IsActionJustPressed("q"))
		{
			for (int i = 0; i < GetChildCount(); i++)
			{
				if (GetChild(i).Name != "spawnTimer")
				{
					GetChild(i).QueueFree();
				}
			}
		}
		
	}

	private void OnTimerTimeout()
	{
		if (Enable)
		{
			int startPos = randomNumberGenerator.RandiRange(0, 7);
			for (int i = startPos; i < 8; i++)
			{
				int enemyNumber = randomNumberGenerator.RandiRange(0, Enemys.Count - 1);
				Vector2 pos = SpawnPositions[i] + player.GlobalPosition;
				Entity enemy = Enemys[enemyNumber].Instantiate() as Entity;
				enemy.GlobalPosition = pos;
				AddChild(enemy); 
			}
			timer.Start();
		}
	}
}
