/* 
 * Copyright (c) Microsoft Corporation. All rights reserved. Licensed under the MIT license.
 * See LICENSE in the project root for license information.
 */

namespace GraphAuthSample
{
    using System;
    using System.Net;
    using System.Net.Http.Headers;

    /// <summary>
    /// Class to carry response result from requets sent to Graph
    /// </summary>
    public class ResponseResult
    {
        /// <summary>
        /// Gets or set the status code of response
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or set the content of response
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or set the latency of response
        /// </summary>
        public long Latency { get; set; }

        /// <summary>
        /// Gets or set the eception of response
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the header of response
        /// </summary>
        public HttpResponseHeaders Headers { get; set; }
    }
}
