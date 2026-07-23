Summary of the flow callchain

````
Main()
 └─ new MathServices()                         → object created, MathPerformed = null
 └─ mathserv.MathPerformed += lambda1           → MathPerformed now references lambda1
 └─ mathserv.CalculateNumbers(57.85, 783.76, lambda2)
      └─ calculation(value1, value2)            → runs lambda2 → returns product
      └─ MathPerformed(product)                 → runs lambda1 → Console.WriteLine(...)
 ````