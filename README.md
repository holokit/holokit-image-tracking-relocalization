# Image Tracking Relocalization

This Unity plugin enhances ARFoundation's image tracking capabilities by relocalizing device coordinate systems through tracked image poses. A common challenge with ARFoundation's image tracking is its inability to consistently yield stable results; relying on a single tracing instance often results in significant deviations. To address this, our solution stablizes the image tracking outcome by utilizing a series of consecutively tracked image poses, thereby refining the accuracy of the final result.

The plugin offers versatile relocalization options: external marker based and dynamically rendered marker based methods. These features make it an ideal tool for both single-player and multiplayer AR experiences.

## External Marker

This approach is suited for both single-player and multiplayer projects. It functions by utilizing an external marker image, which, when scanned by one or several devices, allows them to relocalize their coordinate origins to the image's location. This method is straightforward and easy to implement. However, it's important to note that external markers might not always be available or suitable for every scenario.

To facilitate a smooth start, we've included an "External Marker Relocalization" sample in the package. Importing this sample into your project can help you quickly grasp the setup and usage of this feature.

## Dynamically Renderer Marker

In many cases, an external marker might not be readily available, as it's impractical to expect users to carry such markers with them at all times. Addressing this, our plugin offers a solution specifically designed for multiplayer AR experiences. In this setup, the host device dynamically renders a merker image on its screen, which client devices then scan to relocalize their coordinate systems. This method, while more sophisticated due to the involvement of network communications, offers a practical solution for multiplayer scenarios where external markers are not feasible.
