import { Component, ViewChild, ElementRef, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { arWebDesigner } from '@grapecity/ar-designer';
import { JSViewer, createViewer } from '@grapecity/ar-viewer';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: [
        './home.component.css',
        '../../../node_modules/@grapecity/ar-designer/dist/web-designer.css',
        '../../../node_modules/@grapecity/ar-viewer/dist/jsViewer.min.css'
    ],
	encapsulation: ViewEncapsulation.None, 
})

export class HomeComponent implements OnInit {
    private viewer: JSViewer | null = null;

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        arWebDesigner.create('#ar-web-designer', {
            rpx: { enabled: true },
            appBar: { openButton: { visible: true } },
            data: { dataSets: { visible:true, canModify: true }, dataSources: { canModify: true } },
            preview: {
                openViewer: (options: any) => {
                    if (this.viewer) {
                        this.viewer.openReport(options.documentInfo.id);
                        return;
                    }
                    this.viewer = createViewer({
                        element: '#' + options.element,
                        reportService: {
                            url: 'api/reporting',
                        },
                        reportID: options.documentInfo.id
                    });
                }
            }
        });
    }

    ngOnDestroy() {
        this.viewer?.destroy();
        arWebDesigner.destroy('#ar-web-designer');
	}
}