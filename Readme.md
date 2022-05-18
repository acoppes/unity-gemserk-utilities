## Intro

This is a small project with utilities built over Leopotam ecs to be used for personal projects like game jams, etc. And in the future, if it useful for someone, to share and collaborate to improve.

## Roadmap

* Systems ordering
  - For now, just the gameobjects ordering.
  - Could add attributes for update after/before like Unity ecs 
* Delayed creation /destruction?
  - Maybe have a way to lightweight create, like Flyweight pattern, quickly create the object but initialize later on real usage. If entity destroyed before, then it was never initialized.
  - For destruction, we could mark the entity with special component and exclude it in systems if necessary, and have a System running for last to complete destroy.
* Tuples
  - Dependency Injection of tuples? (there is an extension for that, have to test it yet)
* Entity Queries?
    - This is for quickly searching entities matching some criteria, but not only having or not a component but more like checking if some specific value conditions apply. 
* Database join?
* Spawn entities while processing?
    - For example, for firing bullets or for spawning enemies.
* Controllers (logic running from outside the systems)

* Add or Remove entity during update?
  - Not sure if common, normally we would delegate the destruction to another system, like mark it has no more health or the effect is completed and then there is a system to process that.

## Examples

* Nullify references with events