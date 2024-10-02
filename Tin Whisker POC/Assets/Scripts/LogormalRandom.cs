using System;

public class LognormalRandom
{
    private System.Random rand;
    private double mu;
    private double sigma;

    public LognormalRandom(double mu, double sigma, int? seed = null)
    {
        this.mu = mu;
        this.sigma = sigma;
        this.rand = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
    }

    public double GenerateNormalRandom()
    {
        double u1 = this.rand.NextDouble();
        double u2 = this.rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return this.mu + this.sigma * randStdNormal;
    }

    // Method to generate the next random lognormal value
    public double Next()
    {
        return Math.Exp(GenerateNormalRandom());
    }
}
