using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using jmayberry.Spawner;

public class T_WaveManager {
 
    [UnityTest]
    public IEnumerator WaveManager() {
        var gameObject = new GameObject();
        var waveManager = gameObject.AddComponent<TestWaveManager>();
        waveManager.spawnRadiusCenter = gameObject.transform;
        waveManager.spawnRadius = 10f;

        var prefab = new GameObject();
        var spawnClass = prefab.AddComponent<TestWaveManagerClass>();
        waveManager.waves = new TestWave[] {
            new TestWave { count = 1, possible = new TestWaveManagerClass[] { spawnClass }, timeBetweenSpawns = 1f }
        };

        waveManager.Start();

        Assert.True(waveManager.currentWave != null, "No current wave");
        Assert.False(waveManager.wavesFinished, "Waves did not start");

        yield return new WaitForSeconds(waveManager.timeBetweenWaves + waveManager.currentWave.timeBetweenSpawns + 0.1f);

        Assert.True(waveManager.wavesFinished, "Waves are not over");

        Object.DestroyImmediate(gameObject);
        Object.DestroyImmediate(prefab);
    }

    private class TestWaveManager : WaveManagerBase<TestWaveManagerClass, TestWave> {
        public override bool OnSpawn(TestWaveManagerClass spawnling, TestWave wave, int waveIndex, int spawnlingIndex) {
            return true;
        }
    }

    private class TestWave : IWave<TestWaveManagerClass> {
        public int count { get; set; }
        public TestWaveManagerClass[] possible { get; set; }
        public float timeBetweenSpawns { get; set; }
    }

    private class TestWaveManagerClass : MonoBehaviour, ISpawnable {
        public void OnSpawn(object spawner) { }

        public void OnDespawn(object spawner) { }
    }
}
