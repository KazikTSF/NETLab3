# Metody mnożenia macierzy w projekcie

Projekt porównuje wydajność trzech różnych podejść do mnożenia macierzy o rozmiarze $1000 \times 1000$. Oto metody, które zostały zaimplementowane i użyte w teście benchmarkowym:

### 1. Metoda jednowątkowa (`Multiply`)
* **Opis:** Klasyczny, sekwencyjny algorytm mnożenia macierzy.
* **Działanie:** Obliczenia są wykonywane w całości na jednym wątku głównym, procesor przetwarza wiersze i kolumny jeden po drugim.

### 2. Metoda wielowątkowa wysokopoziomowa (`Multiply` z `ParallelOptions`)
* **Opis:** Wykorzystanie wbudowanej w .NET biblioteki TPL (Task Parallel Library).
* **Działanie:** Wykorzystuje `Parallel.For` oraz `ParallelOptions` do określenia maksymalnej liczby wątków (`MaxDegreeOfParallelism`). Środowisko .NET automatycznie i dynamicznie rozdziela pracę (przetwarzanie wierszy) pomiędzy wątki z puli (Thread Pool).

### 3. Metoda wielowątkowa niskopoziomowa (`MultiplyLowLevel`)
* **Opis:** Ręczne i bezpośrednie zarządzanie wątkami systemowymi.
* **Działanie:** Algorytm sam oblicza zakresy wierszy dla każdego wątku (dzieląc ich liczbę przez liczbę wątków i uwzględniając resztę z dzielenia). Następnie ręcznie tworzy obiekty typu `Thread`, uruchamia je za pomocą `.Start()` i czeka na zakończenie ich pracy metodą `.Join()`.
