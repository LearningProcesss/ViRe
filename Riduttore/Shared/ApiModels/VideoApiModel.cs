public sealed record VideoApiModel(Guid Id,
                                   string? Name,
                                   string? Extension,
                                   string? NativeVaultPath,
                                   string? NativePhysicalVaultPath,
                                   string? NativeSizeReadable,
                                   string? ProcessedVaultPath,
                                   string? ProcessedPhysicalVaultPath,
                                   string? ProcessedSizeReadable,
                                   string? ThumbnailVaultPath,
                                   string? ThumbnailPhysicalVaultPath,
                                   string? Command,
                                   DateTime CreatedOn);

