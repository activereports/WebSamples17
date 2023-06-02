import * as React from "react";

import { JSViewer, createViewer} from '@grapecity/ar-viewer';
import "@grapecity/ar-viewer/dist/jsViewer.min.css";

export const ReportViewer: React.FC<{ reportId: string }> = ({ reportId }) => {
  const [viewerInstance, setViewerInstance] = React.useState<JSViewer | undefined>(undefined);
  React.useEffect(() => {
    if (!viewerInstance) {
      const viewer = createViewer({
        element: "#viewerContainer",
      });
      setViewerInstance(viewer);
    }
    if (reportId) {
      viewerInstance?.openReport(reportId);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [reportId]);
  return <div id="viewerContainer" />;
};
