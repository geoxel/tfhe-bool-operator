Doing boolean operations with both encyphered data and an encyphered boolean operator.

FHE enables us to do boolean operations on encrypted data: bit-and two encrypted values, bit-xor two encrypted values, etc. 
But what if the "operation" itself could be encrypted?
We wouldn't have clear boolean operations:
```
c = a & b
d = a ^ b
```
But instead an encrypted boolean operator `@`, unknown from the outside, stored in a third fhe-encrypted value:
```
c = a @ b
```
This could be fun.
## Setup
Retrieving the TFHE repo and build it with the C API.
```bash
$ git clone https://github.com/zama-ai/tfhe-rs.git
$ cd tfhe-rs
$ RUSTFLAGS="-C target-cpu=native" cargo +nightly build --release --features=high-level-c-api -p tfhe
$ ls -lF target/release
$ cd ..
```
Retrieve the fhe-dotnet repo and build it.
```bash
$ git clone https://github.com/geoxel/fhe-dotnet.git
$ cd fhe-dotnet
$ dotnet build
$ cd ..
```
## Testing
Checkout this repo and run the test:
```bash
$ git clone https://github.com/geoxel/fhe-bool-operator.git
$ cd fhe-bool-operator
$ dotnet run
```
## Following up
Addition of two values can be done with the following loop, using AND, SHIFT and XOR operators.
Unfortunately, this kind of pattern, even if the boolean operators are encyphered, could be read as a clear addition. 
```
uint8_t add(uint8_t a, uint8_t b)
{
    while (b != 0)
    {
        uint8_t carry = (a & b) << 1;
        a = a ^ b;
        b = carry;
    }
    return a;
}
```
Additional ideas have still to be found to hide higher lever operators.

*What a time to be alive!*
