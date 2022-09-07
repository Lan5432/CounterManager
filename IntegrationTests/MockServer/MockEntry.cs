using WireMock.Matchers.Request;

namespace IntegrationTests.MockServer {

    /// <summary>
    /// As it stands Wiremock.Net can't tell two mocks of the same URL apart even from the contents, so it will pick whichever at random.
    /// To avoid this we register mock entries given path and body with a GUID to precisely override the mocks.
    /// </summary>
    internal record struct MockEntry(string path, HttpMethod method) {
    }
}