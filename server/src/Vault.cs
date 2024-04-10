public class Vault : Inventory
{

	//Key is cost, value is size
	public static readonly KeyValuePair<int, int>[] SIZES = new KeyValuePair<int, int>[]
	{
		new(100, 15),
		new(200, 30),
		new(400, 60),
		new(600, 90),
		new(800, 120),
		new(1200, 150),
		new(1500, 200),
		new(2000, 250),
		new(3000, 350),
		new(5000, 500),
		new(7500, 650),
		new(10000, 850),
		new(int.MaxValue, 850)
	};

	public int level;

	public Vault() : base()
	{
		//Default constructor for deserialization
	}

	public Vault(int level) : base(SIZES[level].Value)
	{
		this.level = level;
	}

	public void CalculateStats()
	{
		maxWeight = SIZES[level].Value;
	}

	public string GetText()
	{
		return GetText($"Tier {level + 1} Vault");
	}

}