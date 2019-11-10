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
    ulong[] Code = new ulong[]
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

### Polecenia
- LOAD_MQ
- LOAD_MQ_M
- STOR_M
- LOAD_M
- LOAD_DM
- LOAD_M_M
- LOAD_D_M_M

- STOR_M_L
- STOR_M_R
- JUMP_M_L
- JUMP_M_R
- JUMP_L *
- JUMP_R *

- JUMP_P_M_L
- JUMP_P_M_R
- JUMP_P_L *
- JUMP_P_R *

- ADD_M
- ADD_M_M
- SUB_M
- SUB_M_M
- MUL_M
- DIV_M
- LSH
- RSH

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
