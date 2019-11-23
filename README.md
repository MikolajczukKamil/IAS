# Maszyna IAS
Symulator maszyny IAS

### C#

```C#
int n = 5;
int x = 0;

for (int i = 1; i < N; i++)
  x += i;
           
```

### C# like IAS

```C#
int n = 5;
int x = 0;

int i = 0;

n--;
while(n - i >= 0) {
  i++;
  x += i;
}
```

## IAS

```C#
using IAS;

class MyProject : IAS_Codes {
  static void Main() {
    long[] Code = new long[]
    {
      Word(1), // n = M(0) <>       // 0
      Word(1),                      // 1 // const 1 = M(1)
      Word(0), // i = M(2)          // 2
      Word(0), // x = M(3)          // 3
      Word(
        Instruction(LOAD_M, 0),     // 4L // AC = n
        Instruction(SUB_M, 1)       // 4R // AC--
      ),
      Word(
        Instruction(STOR_M, 0),     // 5L // n = AC 
        Instruction(LOAD_M, 0)      // 5R // AC = n ; while(n - i >= 0)

      ),
      Word(
        Instruction(SUB_M, 2),       // 6L // AC = AC - i
        Instruction(JUMP_P_R, 7)     // 6R // if(AC >= 0) jump to 7R
      ),
      Word(
        Instruction(JUMP_L, 7),      // 7L // else done = inf loop = jump to 7L
        Instruction(LOAD_M, 2)       // 7R // AC = i
      ),
      Word(
        Instruction(ADD_M, 1),       // 8L // AC++
        Instruction(STOR_M, 2)       // 8R // i = AC
      ),
      Word(
        Instruction(ADD_M, 3),       // 9L // AC += x
        Instruction(STOR_M, 3)       // 9R // x = AC
      ),
      Word(
        Instruction(JUMP_R, 5),      // 10L // end while
        0
      )
    };

    IAS_Machine Machine = new IAS_Machine(Code);

    Machine.ManualJumpTo(4); // Program starts at m[4]

    Console.WriteLine(Machine.ToString(4)); // Show 4 first words in memory - m[0-3]

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
1) suma liczb od 1 do n + 1, max 1'048'574
2) suma liczb od 1 do n, max 1'048'575
3) wyrażenie n(n+1)/2, max 1'048'575
4) n!, max 14
5) Kwadrat liczby, max 741'455
6) N wyraz ciągu Fibonacciego, max 57
7) NWD, algorytm Euklidesa, max 549'755'813'887
8) Suma kwadratów do n z użyciem funkcji, max 11'814
9) Dzielenie przez zero
