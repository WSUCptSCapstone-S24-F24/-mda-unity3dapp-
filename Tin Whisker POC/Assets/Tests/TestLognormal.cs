
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System;

public class TestLognormal
{
    // // Test to ensure generated values are always positive
    // [Test]
    // public void TestLognormalSimplePasses()
    // {
    //     // Arrange
    //     double mu = 0.0;
    //     double sigma = 1.0;
    //     LognormalRandom lognormal = new LognormalRandom(mu, sigma);

    //     // Act
    //     double value = lognormal.Next();

    //     // Assert
    //     Assert.Greater(value, 0.0);
    // }

    // // Test to verify statistical properties of the generated values
    // [Test]
    // public void TestLognormalStatistics()
    // {
    //     // Arrange
    //     double mu = 0.0;
    //     double sigma = 1.0;
    //     LognormalRandom lognormal = new LognormalRandom(mu, sigma);

    //     int sampleSize = 10000;
    //     double sum = 0.0;

    //     for (int i = 0; i < sampleSize; i++)
    //     {
    //         sum += lognormal.Next();
    //     }

    //     // Calculate the sample mean
    //     double sampleMean = sum / sampleSize;

    //     // Expected mean for lognormal distribution
    //     double expectedMean = Math.Exp(mu + (sigma * sigma) / 2);

    //     // Assert
    //     Assert.AreEqual(expectedMean, sampleMean, 0.1 * expectedMean);
    // }

    // // Test to ensure reproducibility when using the same seed
    // [Test]
    // public void TestLognormalSeedReproducibility()
    // {
    //     // Arrange
    //     double mu = 0.0;
    //     double sigma = 1.0;
    //     int seed = 42;
    //     LognormalRandom generator1 = new LognormalRandom(mu, sigma, seed);
    //     LognormalRandom generator2 = new LognormalRandom(mu, sigma, seed);

    //     // Act
    //     double value1 = generator1.Next();
    //     double value2 = generator2.Next();

    //     // Assert
    //     Assert.AreEqual(value1, value2);
    // }

    // // Unity test to validate frame-safe generation of lognormal values
    // [UnityTest]
    // public System.Collections.IEnumerator TestLognormalWithEnumeratorPasses()
    // {
    //     // Arrange
    //     double mu = 0.0;
    //     double sigma = 1.0;
    //     LognormalRandom lognormal = new LognormalRandom(mu, sigma);

    //     // Act
    //     double value = 0.0;
    //     yield return new WaitForSeconds(0.1f); // Simulate a frame delay
    //     value = lognormal.Next();

    //     // Assert
    //     Assert.Greater(value, 0.0);
    // }
}

