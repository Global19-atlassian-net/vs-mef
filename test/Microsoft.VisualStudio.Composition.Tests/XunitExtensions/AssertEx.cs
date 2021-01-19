﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.VisualStudio.Composition.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Xunit-like assertions that throw a serializable exception.
    /// </summary>
    /// <remarks>
    /// Since the exceptions thrown by xUnit asserts are not serializable we lose information
    /// in case tests fail in another appdomain. These four helpers throw a serializable exception
    /// that results in a more information exception in the error log.
    /// </remarks>
    internal static class AssertEx
    {
        internal static void False(bool condition, [CallerFilePath] string? filePath = null, [CallerMemberName] string? memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (condition)
            {
                var message = $"Assert failed: expected false (actual: true) at {filePath}, {memberName} line {lineNumber}";
                throw new AssertFailedException(message);
            }
        }

        internal static void True(bool condition, [CallerFilePath] string? filePath = null, [CallerMemberName] string? memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (!condition)
            {
                var message = $"Assert failed: expected true (actual: false) at {filePath}, {memberName} line {lineNumber}";
                throw new AssertFailedException(message);
            }
        }

        internal static void NotNull(object? reference, [CallerFilePath] string? filePath = null, [CallerMemberName] string? memberName = null, [CallerLineNumber] int lineNumber = 0)
        {
            if (reference == null)
            {
                var message = $"Assert failed: unexpected null at {filePath}, {memberName} line {lineNumber}";
                throw new AssertFailedException(message);
            }
        }

        internal static void NotEqual<T>(T expected, T actual, [CallerFilePath] string? filePath = null, [CallerMemberName] string? memberName = null, [CallerLineNumber] int lineNumber = 0)
            where T : IEquatable<T>
        {
            if (EqualityComparer<T>.Default.Equals(expected, actual))
            {
                var message = $"Assert failed. Not expected: {expected} actual: {actual} at {filePath}, {memberName} line {lineNumber}";
                throw new AssertFailedException(message);
            }
        }

        internal static void Equal<T>(T expected, T actual, [CallerFilePath] string? filePath = null, [CallerMemberName] string? memberName = null, [CallerLineNumber] int lineNumber = 0)
            where T : IEquatable<T>
        {
            if (!EqualityComparer<T>.Default.Equals(expected, actual))
            {
                var message = $"Assert failed. Expected: {expected} actual: {actual} at {filePath}, {memberName} line {lineNumber}";
                throw new AssertFailedException(message);
            }
        }

        internal static void Empty<T>(IEnumerable<T> enumerable)
        {
            var collection = enumerable.ToList();
            if (collection.Count > 0)
            {
                throw new AssertFailedException($"Expected empty enumerable but found: [{string.Join(", ", collection)}]");
            }
        }
    }
}
