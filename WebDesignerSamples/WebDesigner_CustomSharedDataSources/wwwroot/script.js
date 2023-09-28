import { arWebDesigner } from './web-designer.js';
import { createViewer } from './jsViewer.min.js';

let viewer = null;
arWebDesigner.create('#ar-web-designer', {
    appBar: { openButton: { visible: true } },
    data: { dataSets: { canModify: true }, dataSources: { canModify: true, shared: { enabled: true }} },
    preview: {
        openViewer: (options) => {
            if (viewer) {
                viewer.openReport(options.documentInfo.id);
                return;
            }
            viewer = createViewer({
                element: '#' + options.element,
                reportService: {
                    url: 'api/reporting',
                },
                reportID: options.documentInfo.id,
                settings: {
                    zoomType: 'FitPage',
                },
            });
        }
    }
});