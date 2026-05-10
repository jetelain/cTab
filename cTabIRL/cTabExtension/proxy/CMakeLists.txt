cmake_minimum_required(VERSION 3.20)
project(cTabExtension_proxy C)

# Proxy DLL that forwards Arma 3 / RvEngine extension calls to the Native AOT implementation.
# Must be built for x86 (win-x86 publish target).

add_library(cTabExtension SHARED cTabExtension_proxy_x86.c)

target_compile_options(cTabExtension PRIVATE /W4 /nologo)

set_target_properties(cTabExtension PROPERTIES
    OUTPUT_NAME "cTabExtension"
    PREFIX      ""
)
