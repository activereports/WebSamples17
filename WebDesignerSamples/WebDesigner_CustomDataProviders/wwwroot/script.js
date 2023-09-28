import { arWebDesigner } from './web-designer.js';
import { createViewer } from './jsViewer.min.js';

let viewer = null;
arWebDesigner.create('#ar-web-designer', {
    rpx: { enabled: true },
    appBar: { openButton: { visible: true } },
    data: { dataSets: { canModify: true }, dataSources: { canModify: true, options: { predefinedProviders: [], customProviders: [
                    { key: 'SQLITE', name: 'SQLite Provider' },
                    { key: 'ODATA', name: 'C1 ODATA Provider' }
                ]} } },
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