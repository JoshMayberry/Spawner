using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using jmayberry.Spawner;

public class T_Spawner {
    [Test]
    public void UnitySpawner() {
        var prefab = new GameObject();
        prefab.AddComponent<TestUnitySpawnerClass>();
        var spawner = new UnitySpawner<TestUnitySpawnerClass>(prefab.GetComponent<TestUnitySpawnerClass>());

        var spawned = spawner.Spawn();
        Assert.IsNotNull(spawned, "Spawn return nothing");
        Assert.IsTrue(spawned is TestUnitySpawnerClass, "Spawn returned incorrect type");
        Assert.True(spawner.IsActive(spawned), "Spawnling is not active");

        spawner.Despawn(spawned);
        Assert.False(spawner.IsActive(spawned), "Spawnling did not despawn");

        Assert.True(spawner.ActiveCount() == 0, "Spawner still has active spawns");
        spawner.Spawn();
        spawner.Spawn();
        Assert.True(spawner.ActiveCount() == 2, "Spawner did not spawn correctly");
        spawner.DespawnAll();
        Assert.True(spawner.ActiveCount() == 0, "Spawner did not despawn all");
        
        spawner.usePooling = true;
        var firstSpawn = spawner.Spawn();
        spawner.Despawn(firstSpawn);
        var secondSpawn = spawner.Spawn();
        spawner.Despawn(firstSpawn);
        Assert.AreEqual(firstSpawn, secondSpawn, "Pooling did not reuse the same object");

        spawner.usePooling = false;
        spawner.destroyUnpooled = false;
        firstSpawn = spawner.Spawn();
        spawner.Despawn(firstSpawn);
        secondSpawn = spawner.Spawn();
        spawner.Despawn(firstSpawn);
        Assert.AreNotEqual(firstSpawn, secondSpawn, "Not Pooling reused the same object");

        Object.DestroyImmediate(prefab);
    }

    [Test]
    public void CodeSpawner() {
        var spawner = new CodeSpawner<TestCodeSpawnerClass>();

        var spawned = spawner.Spawn();
        Assert.IsNotNull(spawned, "Spawn return nothing");
        Assert.IsTrue(spawned is TestCodeSpawnerClass, "Spawn returned incorrect type");
        Assert.True(spawner.IsActive(spawned), "Spawnling is not active");

        spawner.Despawn(spawned);
        Assert.False(spawner.IsActive(spawned), "Spawnling did not despawn");

        Assert.True(spawner.ActiveCount() == 0, "Spawner still has active spawns");
        spawner.Spawn();
        spawner.Spawn();
        Assert.True(spawner.ActiveCount() == 2, "Spawner did not spawn correctly");
        spawner.DespawnAll();
        Assert.True(spawner.ActiveCount() == 0, "Spawner did not despawn all");

        spawner.usePooling = true;
        var firstSpawn = spawner.Spawn();
        spawner.Despawn(firstSpawn);
        var secondSpawn = spawner.Spawn();
        spawner.Despawn(firstSpawn);
        Assert.AreEqual(firstSpawn, secondSpawn, "Pooling did not reuse the same object");

        spawner.usePooling = false;
        firstSpawn = spawner.Spawn();
        spawner.Despawn(firstSpawn);
        secondSpawn = spawner.Spawn();
        spawner.Despawn(firstSpawn);
        Assert.AreNotEqual(firstSpawn, secondSpawn, "Not Pooling reused the same object");
    }
}

public class TestUnitySpawnerClass : MonoBehaviour, ISpawnable {
    public void OnSpawn(object spawner) { }

    public void OnDespawn(object spawner) { }
}

public class TestCodeSpawnerClass : ISpawnable {
    public void OnSpawn(object spawner) { }

    public void OnDespawn(object spawner) { }
}
