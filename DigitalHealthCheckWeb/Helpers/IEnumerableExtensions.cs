using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalHealthCheckWeb.Helpers
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Gets the first item in the sequence after the search predicate returns true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="search">The search predicate.</param>
        /// <param name="valueWhenNotFound">The value to return when the search predicate could not be satisfied.</param>
        /// <returns>
        /// The next value in the collection after the search predicate returns true 
        /// or the valueWhenNotFound if no such item exists.
        /// </returns>
        public static T NextInSequence<T>(this IEnumerable<T> source, Func<T, bool> search, T valueWhenNotFound = default)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (search is null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            var breakNext = false;

            foreach (var item in source)
            {
                if (breakNext)
                {
                    //We found the item last iteration, return the current one.
                    return item;
                }

                if (search(item))
                {
                    breakNext = true;
                }
            }

            // We either couldn't find the item in the collection, or we did, but it was the last item.
            return valueWhenNotFound;
        }

        /// <summary>
        /// Gets the first item in the sequence after the search predicate returns true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="search">The search predicate.</param>
        /// <param name="valueWhenNotFound">The value to return when the search predicate could not be satisfied.</param>
        /// <returns>
        /// The next value in the collection after the search predicate returns true 
        /// or the valueWhenNotFound if no such item exists.
        /// </returns>
        public static async Task<T> NextInSequence<T>(this IAsyncEnumerable<T> source, Func<T, bool> search, T valueWhenNotFound = default)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (search is null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            var breakNext = false;

            await foreach (var item in source)
            {
                if (breakNext)
                {
                    //We found the item last iteration, return the current one.
                    return item;
                }

                if (search(item))
                {
                    breakNext = true;
                }
            }

            // We either couldn't find the item in the collection, or we did, but it was the last item.
            return valueWhenNotFound;
        }

        /// <summary>
        /// Gets the last item in the sequence before the search predicate returns true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="search">The search predicate.</param>
        /// <param name="valueWhenNotFound">The value to return when the search predicate could not be satisfied.</param>
        /// <returns>
        /// The last value in the collection before the search predicate returns true 
        /// or the valueWhenNotFound if no such item exists.
        /// </returns>
        public static T PreviousInSequence<T>(this IEnumerable<T> source, Func<T, bool> search, T valueWhenNotFound = default)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (search is null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            var previous = valueWhenNotFound;

            foreach (var item in source)
            {
                if (search(item))
                {
                    return previous;
                }

                previous = item;
            }

            // We couldn't find the item in the collection.
            return valueWhenNotFound;

        }

        /// <summary>
        /// Gets the last item in the sequence before the search predicate returns true.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source collection.</param>
        /// <param name="search">The search predicate.</param>
        /// <param name="valueWhenNotFound">The value to return when the search predicate could not be satisfied.</param>
        /// <returns>
        /// The last value in the collection before the search predicate returns true 
        /// or the valueWhenNotFound if no such item exists.
        /// </returns>
        public static async Task<T> PreviousInSequence<T>(this IAsyncEnumerable<T> source, Func<T, bool> search, T valueWhenNotFound = default)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (search is null)
            {
                throw new ArgumentNullException(nameof(search));
            }

            var previous = valueWhenNotFound;

            await foreach (var item in source)
            {
                if (search(item))
                {
                    return previous;
                }

                previous = item;
            }

            // We couldn't find the item in the collection.
            return valueWhenNotFound;

        }
    }
}