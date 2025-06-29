﻿# КР1. Паттерны проектирования.
## Выполнил: Золотухин Глеб, БПИ236

### Общая суть работы модуля

Можно создавать, удалять и просматривать счета, 
производить над ними финансовые операции (пополнения и траты).
Все операции имеют категорию, набор категорий кастомизируем. 
Также в модуле доступна аналитика по счетам.

### Архитектура проекта

Есть три основные сущности: Account, Operation, Category, плюс
перечисление типов операций OperationType. Account и Operation
создаются не напрямую через конструктор, а с помощью фабрики.

В проекте два in memory репозитория - с аккаунтами и с операциями. По сути
баланс аккаунта определяет сумма его исходного баланса и 
amount всех операций. Баланс аккаунта может уходить в отрицательные
значения - в нашем банке, видимо, есть беспроцентное кредитование,
удобно:) 

Вся логика работы с репозиторииями спрятана за фасадом,
сам фасад предоставляет функции с понятными задачами вроде
"верни мне все аккаунты", или "дай мне баланс аккаунта". Для
экспорта данных используется шаблонный метод - в проекте есть
только работа с json, но так как работа ведется с интерфейсом
IFinancialVisitor, в будущем в проект можно добавить и YAML,
и CSV. Вообще в целом большинство сущностей (репозиторий, 
визитор, фасад, команда) имеют интерфейсы - по заветам 
солид и interface segregation. Также в проекте реализован паттерн
Команда с декоратором, тут все просто - операции оборачиваются
в команды, команды прячутся за декоратором, который засекает
время их выполнения и выводит на экран. 

В проект внедрен DI контейнер от microsoft, для него
прописан кастомный конфиг и через него происходит обращение к большинству
сущностей.

### Консольное приложение для тестирования модуля

Было написано консольное приложение с демонстрацией функционала
модуля. Реализован несложный и понятный интерфейс с помощью
nuget пакета Specte.Console, в нём есть несколько уже созданных 
счетов, категорий и типов операций, можно добавлять свои, 
проводить операции со счетами, получать информацию об операциях
в json формате, проводить аналитику по счетам.
