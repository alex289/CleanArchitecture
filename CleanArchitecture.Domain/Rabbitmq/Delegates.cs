using System;
using System.Threading.Tasks;

namespace CleanArchitecture.Domain.Rabbitmq;

public delegate Task<bool> ConsumeEventHandler(ReadOnlyMemory<byte> content);