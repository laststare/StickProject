# Stick project
Stick project представляет собой прототип игры в жанре Hyper Casual, выполненный в архитектуре Composition Root.\
Так же есть [репозиторий](https://github.com/laststare/ZenStickProject) с этой же игрой в архитектуре MVVM.\
За основной референс была взята игра [Stick Hero](https://apps.apple.com/ru/app/stick-hero/id918338898)

Архитектура Composition Root, как и MVVM, осуществляет разделение представления и бизнес-логики. Data Binding реализуется за счет инъекции контекстов типа <code>struct</code>, в основном, состоящих из реактивных свойств и событий (UniRX). 

Схема с разделением по логическим слоям
![Image alt](https://github.com/laststare/StickProject/blob/master/Assets/CompositionTree.png)
