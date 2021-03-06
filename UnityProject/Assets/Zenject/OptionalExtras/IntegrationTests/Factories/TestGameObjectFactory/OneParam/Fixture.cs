﻿using ModestTree;
using UnityEngine;
using Zenject.TestFramework;
using Zenject;

namespace Zenject.Tests.TestGameObjectFactory.OneParams
{
    public class Fixture : ZenjectIntegrationTestFixture
    {
        public GameObject CubePrefab;

        const string GameObjName = "TestObj";

        [Test]
        public void TestPrefabSelfSingle1()
        {
            Container.BindFactory<string, GameObject, CubeFactory>().FromPrefab(CubePrefab).WithGameObjectName(GameObjName);

            AddFactoryUser<CubeFactoryTester>();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, GameObjName, 1);
        }

        [Test]
        public void TestPrefabConcreteSingle1()
        {
            Container.BindFactory<string, UnityEngine.Object, ObjectFactory>().To<GameObject>().FromPrefab(CubePrefab).WithGameObjectName(GameObjName);

            AddFactoryUser<ObjectFactoryTester>();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, GameObjName, 1);
        }

        [Test]
        public void TestPrefabResourceSelfSingle1()
        {
            Container.BindFactory<string, GameObject, CubeFactory>().FromPrefabResource("TestGameObjectFactoryOne/Cube").WithGameObjectName(GameObjName);

            AddFactoryUser<CubeFactoryTester>();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, GameObjName, 1);
        }

        [Test]
        public void TestPrefabResourceConcreteSingle1()
        {
            Container.BindFactory<string, UnityEngine.Object, ObjectFactory>().To<GameObject>().FromPrefabResource("TestGameObjectFactoryOne/Cube").WithGameObjectName(GameObjName);

            AddFactoryUser<ObjectFactoryTester>();

            FixtureUtil.AssertNumGameObjects(Container, 1);
            FixtureUtil.AssertNumGameObjectsWithName(Container, GameObjName, 1);
        }

        void AddFactoryUser<T>()
            where T : IInitializable
        {
            Container.Bind<IInitializable>().To<T>().AsSingle();
            Container.BindExecutionOrder<T>(-100);
        }

        public class ObjectFactoryTester : IInitializable
        {
            readonly ObjectFactory _objectFactory;

            public ObjectFactoryTester(ObjectFactory objectFactory)
            {
                _objectFactory = objectFactory;
            }

            public void Initialize()
            {
                var gameObject = (GameObject)_objectFactory.Create("asdf");

                Assert.IsEqual(gameObject.GetComponentInChildren<Foo>().Value, "asdf");

                Log.Info("ObjectFactory initialized foo successfully");
            }
        }

        public class CubeFactoryTester : IInitializable
        {
            readonly CubeFactory _cubeFactory;

            public CubeFactoryTester(CubeFactory cubeFactory)
            {
                _cubeFactory = cubeFactory;
            }

            public void Initialize()
            {
                var gameObject = _cubeFactory.Create("asdf");

                Assert.IsEqual(gameObject.GetComponentInChildren<Foo>().Value, "asdf");

                Log.Info("CubeFactory initialized foo successfully");
            }
        }

        public class ObjectFactory : Factory<string, UnityEngine.Object>
        {
        }

        public class CubeFactory : Factory<string, GameObject>
        {
        }
    }
}
