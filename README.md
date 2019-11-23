# Maszyna IAS
Symulator maszyny IAS

## C# code

```C#
int N = 5;
int X = 0;

for (int i = 1; i < N; i++)
  X += i;
                
```

## IAS Code

```C#
using IAS;

class MyProject : IAS_Codes {
  static void Main() {
    long[] Code = new long[]
    {
      Word(5), // <>                // 0 N
      Word(1),                      // 1 ++
      Word(0),                      // 2 i
      Word(0),                      // 3 X
      Word(
        Instruction(LOAD_M, 0),     // 4L
        Instruction(SUB_M, 1)       // 4R
      ),
      Word(
        Instruction(SUB_M, 1),      // 5L
        Instruction(STOR_M, 0)      // 5R
      ),
      Word(
        Instruction(LOAD_M, 0),     // 6L
        Instruction(SUB_M, 2)       // 6R
      ),
      Word(
        Instruction(JUMP_P_L, 8),   // 7L
        Instruction(JUMP_R, 7)      // 7R
      ),
      Word(
        Instruction(LOAD_M, 2),     // 8L
        Instruction(ADD_M, 1)       // 8R
      ),
      Word(
        Instruction(STOR_M, 2),     // 9L
        Instruction(ADD_M, 3)       // 9R
      ),
      Word(
        Instruction(STOR_M, 3),     // 10L
        Instruction(JUMP_L, 6)      // 10R
      )
    };

    IAS_Machine Machine = new IAS_Machine(Code);

    Machine.ManualJumpTo(4); // Program starts at m[4]

    Console.WriteLine(Machine.ToString(4)); // Show 4 first words in memory -  m[0-3]

    while(Console.ReadKey().KeyChar != 'x') {
      Machine.Step();

      Console.WriteLine(Machine.ToString(4));
    }
  }
}

```

## Projekt IAS
Zawiera klasę IAS_Machne przyjmującą w konstruktorze kod w postaci UInt64[], 
oraz klasę IAS_Codes która dostarcza stałe zawierające kody instrukcji i 
metody pozwalające łatwo pisać kod IAS

### Rozkazy IAS

| Transfer danych |              |                |
| ------------- | -------------- |--------------- |
| LOAD_MQ       | LOAD MQ        | AC = MQ        |
| LOAD_MQ_M     | LOAD MQM(X)    | MQ = M(X)      |
| STOR_M        | STOR M(X)      | M(X) = AC      |
| LOAD_M        | LOAD M(X)      | AC = M(X)      |
| LOAD_D_M      | LOAD -M(X)     | AC = -M(X)     |
| LOAD_M_M      | LOAD \|M(X)\|  | AC = \|M(X)\|  |
| LOAD_D_M_M    | LOAD -\|M(X)\| | AC = -\|M(X)\| |

| Modyfikacja adresu |              |                                                          |
| -------------- | ---------------- | -------------------------------------------------------- |
| STOR_M_L       | STOR M(X, 8:19)  | zamień adres lewego rozkazu M(X) na 12 prawych bitów AC  |
| STOR_M_R       | STOR M(X, 28:39) | zamień adres prawego rozkazu M(X) na 12 prawych bitów AC |

| Skoki bezwarunkowe |             |                               |
| ------------- | ---------------- | ------------------------------|
| JUMP_M_L      | JUMP M(X, 0:19)  | skocz do lewego rozkazu M(X)  |
| JUMP_M_R      | JUMP M(X, 20:39) | skocz do prawego rozkazu M(X) |
| JUMP_L        | JUMP (X, 0:19) * | skocz do lewego rozkazu X   * |
| JUMP_R        | JUMP (X, 0:19) * | skocz do prawego rozkazu X  * |

| Skoki warunkowe |                    |                                              |
| --------------- | ------------------ | ---------------------------------------------|
| JUMP_P_M_L      | JUMP + M(X, 0:19)  | jeżeli AC >= 0 skocz do lewego rozkazu M(X)  |
| JUMP_P_M_R      | JUMP + M(X, 20:39) | jeżeli AC >= 0 skocz do prawego rozkazu M(X) |
| JUMP_P_L        | JUMP + (X, 0:19) * | jeżeli AC >= 0 skocz do lewego rozkazu X   * |
| JUMP_P_R        | JUMP + (X, 0:19) * | jeżeli AC >= 0 skocz do prawego rozkazu X  * |

| Arytmetyczne |              |                                |
| ------------ | ------------ | -------------------------------|
| ADD_M        | ADD M(X)     | AC = AC + M(X)                 |
| ADD_M_M      | ADD \|M(X)\| | AC = AC + \|M(X)\|             |
| SUB_M        | SUB M(X)     | AC = AC - M(X)                 |
| SUB_M_M      | SUB \|M(X)\| | AC = AC - \|M(X)\|             |
| MUL_M        | MUL M(X)     | AC:MQ = MQ * M(X)              |
| DIV_M        | DIV M(X)     | MQ = AC / M(X); AC = AC % M(X) |
| LSH          | LSH          | AC = AC << 1                   |
| RSH          | RSH          | AC = AC >> 1                   |

\* Polecenia dodane, nie są uwzględniowne w tabeli rozkazów

## Projekt Symulator_IAS

### Kilka przykładowych programów napisanych w IAS
1) Zadanie 1, suma liczb od 1 do n, działająca wersja z wykładu
2) Zadanie 1, suma liczb od 1 do n, poprawna wersja
3) Zadanie 2, wyrażenie n(n+1)/2
4) Zadanie 3, n!, z wykładu
5) Kwadrat liczby
6) N wyraz ciągu Fibonacciego
7) NWD, algorytm Euklidesa
8) Suma kwadratów do n z użyciem funkcji
