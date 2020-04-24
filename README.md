# Maszyna IAS

<p>Symulator maszyny IAS zwanej też maszyną von Neumanna (https://en.wikipedia.org/wiki/IAS_machine)</p>
<p>Oparta na architekturze von Neumana (https://en.wikipedia.org/wiki/Von_Neumann_architecture)</p>

## Przykład, suma od 1 do n

W trzech fazach

- Zaczynamy od współczesnego kodu C#
- W współczesnym języku emitujemy podejście IAS'a poprzez 
  - używanie pętli while
  - używanie zmiennej akumulatora (AC) jako pośrednia działań
  - przeniesienie wszystkich zmiennych i wartości na początek jak
  - użycie tylko porównań z 0
- Zapisujemy kod w postaci czystego IAS

```C#
// C#

int n = 10;
int sum = 0;

for (int i = 1; i <= n; i++)
  sum += i;
```

```C#
// C# like IAS

int AC = 0;

int n = 10;
int sum = 0;
int i = 0;
int one = 1;

while(true) {
  AC = n;         // n
  AC -= i;        // n - 1
  if(AC >= 0) {}  // while(n - i >= 0) <=> while(i <= n)
  else break;
  AC = i;         // i
  AC += sum;      // sum + i
  sum = AC;       // sum = sum+ i
  AC = i;         // i
  AC += one;      // i + 1
  i = AC;         // i = i + 1
}
```

```C#
// IAS

long[] Code = new long[]
{
  Word(n),                      // 0 // n = M(0) <>
  Word(1),                      // 1 // i = M(1)
  Word(0),                      // 2 // x = M(2)
  Word(1),                      // 3 // 1 = M(3), const
  Word(
    Instruction(LOAD_M, 0),     // 4L // AC = n ; while(n - i >= 0)
    Instruction(SUB_M, 1)       // 4R // AC -= i
  ),
  Word(
    Instruction(JUMP_P_L, 6),   // 5L // if(AC >= 0) jump to 6L
    Instruction(JUMP_R, 5)      // 5R // else halt <=> inf loop <=> jump to 5R
  ),
  Word(
    Instruction(LOAD_M, 1),      // 6L // AC = i
    Instruction(ADD_M, 2)        // 6R // AC += x
  ),
  Word(
    Instruction(STOR_M, 2),      // 7L // x = AC
    Instruction(LOAD_M, 1)       // 7R // AC = i
  ),
  Word(
    Instruction(ADD_M, 3),       // 8L // AC++
    Instruction(STOR_M, 1)       // 8R // i = AC
  ),
  Word(
    Instruction(JUMP_L, 4),      // 9L // end while
    0
  )
};
```

### Przykładowy projekt IAS

<details>
  <summary>Ten sam kod tylko umieszczony w prostym projekcie</summary>

  ```C#
  // C#

  using System;
  using IAS;

  class MyFirstIASProject : IAS_Codes {

    static void Main() {
      Console.Write("n = ");
      int n = Convert.ToInt32(Console.ReadLine());

      long[] Code = new long[]
      {
        Word(n),                      // 0 // n = M(0) <>
        Word(1),                      // 1 // i = M(1)
        Word(0),                      // 2 // x = M(2)
        Word(1),                      // 3 // 1 = M(3), const
        Word(
          Instruction(LOAD_M, 0),     // 4L // AC = n ; while(n - i >= 0)
          Instruction(SUB_M, 1)       // 4R // AC -= i
        ),
        Word(
          Instruction(JUMP_P_L, 6),   // 5L // if(AC >= 0) jump to 6L
          Instruction(JUMP_R, 5)      // 5R // else halt <=> inf loop <=> jump to 5R
        ),
        Word(
          Instruction(LOAD_M, 1),      // 6L // AC = i
          Instruction(ADD_M, 2)        // 6R // AC += x
        ),
        Word(
          Instruction(STOR_M, 2),      // 7L // x = AC
          Instruction(LOAD_M, 1)       // 7R // AC = i
        ),
        Word(
          Instruction(ADD_M, 3),       // 8L // AC++
          Instruction(STOR_M, 1)       // 8R // i = AC
        ),
        Word(
          Instruction(JUMP_L, 4),      // 9L // end while
          0
        )
      };
      
      try {
        IAS_Machine Machine = new IAS_Machine(Code);

        Machine.ManualJumpTo(4);                    // Program starts at m[4]

        Console.WriteLine(Machine.ToString(4));     // Show 4 first words in memory - m[0-3]

        while(Console.ReadKey().KeyChar != 'x') {
          Machine.Step();

          Console.WriteLine(Machine.ToString(4));
        }
      } catch(Exception e) {
        Console.WriteLine($"Error: {e.Message}");
      }
    }

  }
  ```
</details>

## Projekt IAS

`IAS_Machne` - model maszyny IAS
- Konstruktor wymaga kodu maszynowego IAS w postaci `UInt64[]`
- Początkową pozycję ustala się za pomocą metody `ManualJumpTo(Address)`, musi być to słowo rozkazu
- Następne instrukcje wykonywane są po każdym wykonaniu metody `Step()`, jest to jakby cykl zegara
- Maszyna kończy pracę wchodząc w nieskończoną pętlę, można to sprawdziś metodą `IsDone()`
- Informacje o stanie maszyny można pobrać w formie opisowej (debugger) za pomocą metody `toString(manyMemoryWordsToShow?)` lub w formie stanu pamięci `GetMemory(copy?)`

`IAS_Codes` - dostarcza stałe zawierające kody instrukcji i metody pozwalające łatwo generować kod IAS
- Metodę `Instruction(OperationCode, Address)`
- Metoda `Word(leftInstruction, rightInstruction)` generuje słowo rozkazu z lewej i prawej instrukcji
- Metody `Word(data)` oraz `Word()` generują słowo danych z podanymi danymi lub puste słowo danych - 0
- Wszystkie stałe wymienione w poniższej tabeli

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

\* Polecenia dodane, nie są uwzględniowne w orginalnej tabeli rozkazów

## Projekt Symulator_IAS

Kilka przykładowych programów napisanych w IAS z debugerem umieszczone w prostym interfejsie tekstowym

1) suma liczb od 1 do n + 1, maksymalna wartość 1 048 574      -- przykład z wykładu z błędem
2) suma liczb od 1 do n, maksymalna wartość 1 048 575          -- przykład z wykładu poprawiony
3) wyrażenie n(n+1)/2, maksymalna wartość 1 048 575            -- przykład z wykładu
4) n!, maksymalna wartość 14                                   -- przykład z wykładu
5) Kwadrat liczby, maksymalna wartość 741 455
6) N wyraz ciągu Fibonacciego, maksymalna wartość 57
7) NWD, algorytm Euklidesa, maksymalna wartość 549 755 813 887
8) Suma kwadratów do n z użyciem funkcji, maksymalna wartość 11 814
9) Przykład z błędem - dzielenie przez zero
