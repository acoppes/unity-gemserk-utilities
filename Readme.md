## Intro

This is a small project with wrapper utilities built over Leopotam ecs to be used for personal projects like game jams, etc. And in the future, if it useful for someone, to share and collaborate to improve.

### World

World is a wrapper over the Leopotam EcsWorld, and it has a prefab with predefined systems in there. As this is our Unity adaptation of the ecs, we treat systems as game objects with a BaseSystem MonoBehaviours.

World contains three objects, FixedUpdate, Update and LateUpdate, systems inside those objects are updated using the corresponding Untiy update type. 

### Entity Definitions & instance parameters (TODO)

* Entity definition
* Entity instantiation + parameters
* World create entity
* EntityDefinitionComponent (just to keep reference, but can be used to clone) 

### Named entities & Singletons

Entities can use the NameComponent to identify them by name, to be used in any way by the game but they can also be marked as singleton which make them unique so there can't be two entities with the same name marked as singletons. Singleton entities are cached in shared object dictionary and can be accessed by name from there. 

## Roadmap

* Systems ordering
  - For now, just the gameobjects ordering.
  - Could add attributes for update after/before like Unity ecs 
* Delayed creation /destruction?
  - Maybe have a way to lightweight create, like Flyweight pattern, quickly create the object but initialize later on real usage. If entity destroyed before, then it was never initialized.
  - For destruction, we could mark the entity with special component and exclude it in systems if necessary, and have a System running for last to complete destroy.
* Tuples
  - ~~Dependency Injection of tuples? (there is an extension for that, have to test it yet)~~
  - A concept between the filter and the pools? When we programmed Clash of the Olympians we had something like that. 
* Entity Queries?
    - This is for quickly searching entities matching some criteria, but not only having or not a component but more like checking if some specific value conditions apply. 
* Database join?
* Spawn entities while processing?
    - For example, for firing bullets or for spawning enemies.
* Controllers (logic running from outside the systems)

* Add or Remove entity during update?
  - Not sure if common, normally we would delegate the destruction to another system, like mark it has no more health or the effect is completed and then there is a system to process that.

* ~~Maybe use gameobjects to identify fixed update, update and late update systems and remove that from code.~~

## Examples

TODO: 

* Nullify references with events.

### SpawnEntitiesExample scene

Shows how to dynamically create entities while iterating in a System and to configure stuff. In this case it is a weapon with a cooldown spawning bullets each time that cooldown is ready (no visual stuff)

### ReferenceToGameObjects scene

Shows how to dynamically create and destroy a Unity GameObject and keep data in sync. It is normally useful in the case of having to use a GameObject to use Unity stuff like SpriteRenderer, etc.